namespace LearnFast.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper.Execution;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.CustomerService;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using NuGet.Protocol.Plugins;
    using Stripe;
    using Stripe.Checkout;
    using Stripe.Issuing;


    public class PaymentController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IFilterCourse filterCourse;
        private readonly IPaymentService paymentService;
        private readonly IFilterCourse courseService;

        public PaymentController(
            IConfiguration configuration,
            IFilterCourse filterCourse,
            IPaymentService paymentService,
            IFilterCourse courseService)
        {
            this.configuration = configuration;
            this.filterCourse = filterCourse;
            this.paymentService = paymentService;
            this.courseService = courseService;
        }

        public async Task<IActionResult> CreateAccount()
        {
            var refreshUrl = this.Url.Action("IsPaid", CourseController.CourseNameController, null, protocol: "https");
            var returnUrl = this.Url.Action("Create", CourseController.CourseNameController, values: new { isPaid = true }, protocol: "https");
            var linkResponse = await this.paymentService.CreateAccountAsync(refreshUrl, returnUrl);

            // The stripe account exists, therefore redirecting to create the course page.
            if (linkResponse == string.Empty)
            {
                return this.Redirect(returnUrl);
            }

            return this.Redirect(linkResponse);
        }

        // buy a course
        public async Task<IActionResult> Buy(int courseId)
        {
            var successUrl = this.Url.Action("Details", CourseController.CourseNameController, values: new { id = courseId }, protocol: "https");
            var cancelUrl = this.Url.Action("InvalidPaid", "Payment", null, protocol: "https");
            var linkResponse = await this.paymentService.BuyProductAsync(courseId, successUrl, cancelUrl);

            return this.Redirect(linkResponse);
        }

        public IActionResult InvalidPaid()
        {
            return this.View();
        }
    }
}
