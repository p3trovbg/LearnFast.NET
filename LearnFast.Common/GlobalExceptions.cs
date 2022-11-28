namespace LearnFast.Common
{
    public static class GlobalExceptions
    {
        public const string LanguageNullExceptionMessage = "This language does not exist!";

        public const string CategoryNullExceptionMessage = "This category does not exist!";

        public const string CourseDoesNotExistExceptionMessage = "This course does not exist!";

        public const string VideoDoesNotExistExceptionMessage = "This video does not exist!";

        public const string DoesNotOwnThisCourseExceptionMessage = "You do not own this course!";

        public const string DoesNotExistReviews = "Not have reviews yet.";

        public const string DoesNotExistReview = "This review does not exist!";

        public const string LimitOfSelectedReviews = "Limit of the selected reviews is 5";

        public const string UserNotExists = "This user does not exist!";

        public const string UserNotHasPermission = "This user doesn't has permission";

        public const string InvalidUsername = "Invalid username";

        public const string ConfirmedPasswordNotMatch = "The password and confirmation password do not match.";

        public const string UserAlreadyHasEnrolledInCourse = "This user already has enrolled in the course.";
    }
}
