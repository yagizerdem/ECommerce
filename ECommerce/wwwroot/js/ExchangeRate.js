var loop;
async function ExchangeRates() {
    const response = await fetch("https://api.vatcomply.com/rates?base=USD")
    const data = await response.json()
    CreateSlider(data.rates)
}
ExchangeRates()

function CreateSlider(rates) {

    const slider = document.getElementById("splide__list")
    for (const [key, value] of Object.entries(rates)) {
        slider.innerHTML += `<li class="splide__slide"><span class="exchange-rates">${key + ':' + value}</span></li>`
    }
    DefaultAPIforSlider()
    AutoScrol()
}
function DefaultAPIforSlider() {
    var splide = new Splide('.splide', {
        type: 'loop',
        perPage: 3,
        autoplay: true,
    });

    splide.mount();
}
function AutoScrol() {
    const left_arrow = document.getElementsByClassName("splide__arrow--next")[0]
    loop = setInterval(() => {
        left_arrow.click()
    }, 2000)
}