const form = document.getElementById("form");

const firstName = document.getElementById("firstName");
const lastName = document.getElementById("lastName");
const email = document.getElementById("email");
const phone = document.getElementById("phone");
const message = document.getElementById("message");

const form_error = document.getElementById("form_error");

const formSubmitBtn = document.getElementById("formSubmitBtn");

const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
const phoneRegex = /^[0-9]{10}$/;
const nameRegex = /^(?:[\u00C0-\u01BF\u01C4-\u024F\u1E00-\u1EFFa-zA-Z'’-]+\s?)+$/u;
const textRegex = /^(?:[\u00C0-\u01BF\u01C4-\u024F\u1E00-\u1EFFa-zA-Z0-9'’!?@&-]+\s?)+$/u;

let inputsPassed = false;
let inputsValidity = {
    email: false,
    phone: false,
    firstName: false,
    lastName: false,
    message: false
};

const addInputEventListener = (element, regex, maxLength, errorMessage) => {
    element.addEventListener("input", () => {
        if (element === phone) {
            element.value = element.value.replace(/\D/g, '');
        }
        const valid = Validateinput(element.value, regex, maxLength);
        element.style.border = "1px solid " + InputColor(valid);
        form_error.textContent = valid ? "" : errorMessage;
        inputsValidity[element.id] = valid;
        const allInputsValid = Object.values(inputsValidity).every(inputValid => inputValid);       // Calculate allInputsValid after updating inputsValidity
        formSubmitBtn.classList = allInputsValid ? "btn" : "btn btn--disabled";
        formSubmitBtn.disabled = !allInputsValid;                                                    // Invert the condition since we are checking if all inputs are valid
    });
};


addInputEventListener(email, emailRegex, 128, "Please enter a valid email address.");
addInputEventListener(phone, phoneRegex, 50, "Please enter a valid phone number (06 12345678).");
addInputEventListener(firstName, nameRegex, 128, "Please enter a valid first name.");
addInputEventListener(lastName, nameRegex, 128, "Please enter a valid last name.");
addInputEventListener(message, textRegex, 600, "Please enter a valid message (max 600 letters).");



form.addEventListener("submit", async (event) => {
    event.preventDefault();
    ToggleLoader(true);
    phone.value = phone.value.replace(/\D/g, '');
    const inputsPassed = [
        { element: email, regex: emailRegex, maxLength: 128 },
        { element: phone, regex: phoneRegex, maxLength: 50 },
        { element: firstName, regex: nameRegex, maxLength: 128 },
        { element: lastName, regex: nameRegex, maxLength: 128 },
        { element: message, regex: textRegex, maxLength: 600 }
    ].every(({ element, regex, maxLength }) => {
        element.style.border = "1px solid " + InputColor(Validateinput(element.value, regex, maxLength));
        return Validateinput(element.value, regex, maxLength);
    });

    if (inputsPassed) {
        console.log("regex passed trying to submit.");
        grecaptcha.ready(() => {
            grecaptcha.execute('6LcUPHgpAAAAAH41iAD2fYHlWRzzLM4IDDkk1Dy3', { action: 'submit' })
                .then(submitContactForm)
                .catch((error) => formError('Error submitting form:', error.error))
        });
    } else {
        formError('not all fields are valid.');
    }
});


async function submitContactForm(token) {
    const formData = { firstName: firstName.value, lastName: lastName.value, email: email.value, phone: phone.value, message: message.value, recaptchaToken: token };

    try {
        const response = await fetch('../api/forms/contact', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(formData) });
        const data = await response.json();
        if (data.status != "success") throw new Error(data.error)
        form.reset();
        formSuccess();
    } catch (error) {
        formError('Error submitting form:', error.message)
    }
}




function Validateinput(input, regex, maxlenght = null) {
    if (regex.test(input) && input !== "") {
        if (maxlenght !== null) {
            if (input.length >= maxlenght) {
                inputsPassed = false;
                return false;
            }
        }
        return true;
    } else {
        inputsPassed = false;
        return false;
    }
}

function InputColor(valid) {
    return valid ? "transparent" : "red";
}
function isNotEmpty(input) {
    return input !== "";
}

function formError(message) {
    ToggleLoader(false);
    let formSubmit_error = document.getElementById("formSubmit_error");
    formSubmit_error.innerHTML = message;
    const formErrorElement = document.getElementById("formError");
    formErrorElement.style.animation = "none"; // Resetting animation
    formErrorElement.style.display = "flex";
    formErrorElement.style.animation = "notify 5s 1 ease-in-out"; // Restarting animation
    setTimeout(() => {
        formErrorElement.style.display = "none";
    }, 5000);
}

function formSuccess() {
    ToggleLoader(false);
    const formSuccessElement = document.getElementById("formSuccess");
    formSuccessElement.style.animation = "none"; // Resetting animation
    formSuccessElement.style.display = "flex";
    formSuccessElement.style.animation = "notify 5s 1 ease-in-out"; // Restarting animation
    setTimeout(() => {
        formSuccessElement.style.display = "none";
    }, 5000);
}
