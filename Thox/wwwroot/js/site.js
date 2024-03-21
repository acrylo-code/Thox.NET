// Please see doconsole.log("JavaScript file loaded");

document.addEventListener('DOMContentLoaded', function () {
    // Check if the current URL contains "contact/me"
    if (window.location.href.indexOf("Contact/me") > -1 || window.location.href.indexOf("contact/me") > -1) {
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

//when a page is loading show the loader
document.addEventListener('DOMContentLoaded', function () {
    //if the page takes longer than 1 second to load show the loader
    ToggleLoader(true);
});

//when the page is done loading hide the loader
window.onload = function () {
    ToggleLoader(false);
};
