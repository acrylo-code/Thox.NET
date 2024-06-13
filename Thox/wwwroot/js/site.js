// Please see doconsole.log("JavaScript file loaded");

document.addEventListener('DOMContentLoaded', function () {
    // Check if the current URL contains "info_container/me"
    if (window.location.href.indexOf("info_container/me") > -1 || window.location.href.indexOf("info_container/me") > -1) {
        const hoverTargets = document.querySelectorAll('.hoverTarget');
        const hiddenDiv = document.getElementById('hiddenDiv');
        const h1Element = hiddenDiv.querySelector('h3');
        const pElement = hiddenDiv.querySelector('p');
        const starsElement = document.getElementById('star-rating');

        hoverTargets.forEach(hoverTarget => {
            const titleText = hoverTarget.getAttribute('data-title');
            const bodyText = hoverTarget.getAttribute('data-text');
            const stars = parseInt(hoverTarget.getAttribute('data-stars'));

            hoverTarget.addEventListener('mouseout', () => {
                if (h1Element) {
                    h1Element.textContent = "Over mij";
                }
                if (pElement) {
                    pElement.textContent = "Ik ben een enthousiaste beginnende programmeur met ervaring in veel verschillende velden van Ict.";
                }
                starsElement.classList.remove('visible');
            });

            hoverTarget.addEventListener('mouseover', () => {
                if (h1Element) {
                    h1Element.textContent = titleText;
                }
                if (pElement) {
                    pElement.textContent = bodyText;
                }
                starsElement.classList.add('visible');

                const ratingStars = hiddenDiv.querySelectorAll('.master_star');
                ratingStars.forEach((star, index) => {
                    if (index < stars) {
                        star.classList.add('masterd');
                    } else {
                        star.classList.remove('masterd');
                    }
                    star.classList.add('visible');
                });
            });
        });
    }
});

function ToggleLoader(state) {
    let loader = document.getElementById("site_loader");
    if (state) {
        loader.style.display = "flex";
        loader.style.animation = "fadeIn 0.1s ease-in-out 1";
    } else {
        loader.style.animation = "fadeOut 1s ease-in-out 1";
        setTimeout(() => {
            loader.style.display = "none";
        }, 1000); // Adjust the delay to match the duration of the animation
    }
}

function animateNumbers(element) {
    const targetNumber = parseFloat(element.dataset.number);
    const decimalPlaces = targetNumber % 1 !== 0 ? targetNumber.toString().split('.')[1].length : 0;
    const step = targetNumber / 100;
    let currentNumber = 0;

    const observer = new IntersectionObserver(entries => {
        if (entries[0].isIntersecting) {
            const interval = setInterval(() => {
                currentNumber += step;

                // Round currentNumber to the original decimal places
                currentNumber = parseFloat(currentNumber);

                // Display the current number as text inside the element
                if (decimalPlaces > 0) {
                    element.textContent = currentNumber.toLocaleString(undefined, { minimumFractionDigits: decimalPlaces, maximumFractionDigits: decimalPlaces });
                } else {
                    element.textContent = currentNumber.toFixed(decimalPlaces).toLocaleString();
                }

                if (currentNumber >= targetNumber) {
                    clearInterval(interval);
                    // Ensure the final number matches the original data-number value
                    element.textContent = targetNumber.toLocaleString(undefined, { minimumFractionDigits: decimalPlaces, maximumFractionDigits: decimalPlaces });
                }
            }, 20); // Adjust the interval (milliseconds) for smoother animation

            observer.disconnect();
        }
    });
    observer.observe(element);
}

// Get the elements with the .number-animate class
const numberElements = document.querySelectorAll('.number-animate');
numberElements.forEach(animateNumbers);


//when a page is loading show the loader
document.addEventListener('DOMContentLoaded', function () {
    //if the page takes longer than 1 second to load show the loader
    ToggleLoader(true);
});

//when the page is done loading hide the loader
window.onload = function () {
    ToggleLoader(false);
};

document.addEventListener("DOMContentLoaded", function() {
    const currentYear = new Date().getFullYear();
document.getElementById("currentYear").textContent = currentYear;
});

document.addEventListener('mousemove', function(e) {
    var footer = document.querySelector('.footer_container');
    var viewportHeight = window.innerHeight;
    var threshold = 50; // Distance from the bottom of the viewport to trigger the footer

    if (viewportHeight - e.clientY < threshold) {
        footer.style.bottom = '0';
    } else {
        footer.style.bottom = '-40px';
    }
});
