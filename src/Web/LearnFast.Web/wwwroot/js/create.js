function auto_height(area) {
    area.style.height = "100px";
    area.style.height = (area.scrollHeight) + "px";
}

document.getElementById('upload').addEventListener('change', function () {
    if (this.files[0]) {
        var picture = new FileReader();
        picture.readAsDataURL(this.files[0]);
        picture.addEventListener('load', function (event) {
            document.getElementById('uploadedImage').setAttribute('src', event.target.result);
            document.getElementById('uploadedImage').style.display = 'block';
        });
    }
});