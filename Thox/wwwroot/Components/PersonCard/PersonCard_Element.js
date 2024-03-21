try {
    fetch('../../Components/PersonCard/PersonCard_Element.html')
        .then(response => response.text())
        .then(html => {
            // Define the custom element outside the loop, just once
            class PersonCard extends HTMLElement {
                constructor() {
                    super();
                }
                connectedCallback() {
                    const personCard_template = document.createElement('template'); // create a new template for each person-card

                    // Assign HTML content to the template
                    personCard_template.innerHTML = html;

                    let personCount = parseInt(this.getAttribute('personcount'));

                    // Find and replace placeholders in the template
                    const contentClone = personCard_template.content.cloneNode(true);
                    contentClone.querySelector('.cardtitle').textContent = this.getAttribute('title');
                    contentClone.querySelector('.image').setAttribute('src', "../" + this.getAttribute('image'));
                    contentClone.querySelector('.personCount').textContent = personCount + " personen";
                    contentClone.querySelector('.line-text').textContent = parseInt(this.getAttribute('successpercentage')) + '% slagingspercentage';
                    contentClone.querySelector('.progress-line__bar').style.width = parseInt(this.getAttribute('successpercentage')) + '%';
                    contentClone.querySelector('.price').textContent = parseFloat(this.getAttribute('price')) + ".-";
                    //contentClone.querySelector('.link').setAttribute('href', "/reservation/dateselection?PersonCount=" + parseInt(this.getAttribute('personcount')));

                    //when a card is clicked, navigate to the reservation page and POST the personcount
                    contentClone.querySelector('.link').addEventListener('click', function () {
                        fetch("/api/reservation/personCount", {
                            method: "POST",
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify({ PersonCount: personCount })
                        })
                            .then(response => response.json())
                            .then(data => {
                                if (data.status == "success") {
                                    window.location.href = "/reservation/dateselection?" + data.link;
                                }
                            })
                            .catch((error) => {
                                console.error('Error:', error);
                            });
                    });

                    const shadowRoot = this.attachShadow({ mode: 'open' });
                    shadowRoot.appendChild(contentClone);
                }
            }

            // Define the custom element
            customElements.define('person-card', PersonCard);

            //select all person-card elements
            const cardList = document.querySelectorAll('.person-card');

            //loop through each person-card element
            cardList.forEach(personCard => {
                // Instantiate the custom element for each person-card
                customElements.upgrade(personCard);
                //remove the atributes from the person-card element
                personCard.removeAttribute('title');
                personCard.removeAttribute('image');
                personCard.removeAttribute('personcount');
                personCard.removeAttribute('successpercentage');
                personCard.removeAttribute('price');
                //and the class
                personCard.classList.remove('person-card');

            });
        })
        .catch(error => console.error('Error fetching HTML file:', error));
} catch (error) {
    console.error('Error fetching HTML file:', error);
}




//const personCard_template = document.createElement('template');