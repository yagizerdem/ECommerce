window.onload = () => {
    tinymce.init({
        selector: '.wsyeditor',
        plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount',
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table | align lineheight | numlist bullist indent outdent | emoticons charmap | removeformat',
    });

    const sliderEl = document.querySelector("#range")
    const sliderValue = document.querySelector(".value")

    sliderEl.addEventListener("input", (event) => {
        const tempSliderValue = event.target.value;

        sliderValue.textContent = tempSliderValue;

        const progress = (tempSliderValue / sliderEl.max) * 100;

        sliderEl.style.background = `linear-gradient(to right, #f50 ${progress}%, #ccc ${progress}%)`;
    })

}