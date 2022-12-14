document.getElementById('upload').addEventListener('change', function () {
    if (this.files[0]) {
        var video = new FileReader();
        video.readAsDataURL(this.files[0]);
        video.addEventListener('load', function (event) {
            document.getElementById('uploadedVideo').setAttribute('src', event.target.result);
            document.getElementById('uploadedVideo').style.display = 'block';
        });
    }
});