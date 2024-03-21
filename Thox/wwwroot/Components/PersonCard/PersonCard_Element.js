try {
    fetch('../../Components/PersonCard/PersonCard_Element.html')
        .then(response => response.text())
        .then(html => {
            // Define the custom element
            class PersonCard extends HTMLElement {
                constructor() {
                    super();
                }

                connectedCallback() {
                    // Parse HTML content into a template
                    const template = document.createElement('template');
                    template.innerHTML = html;

                    // Get attributes
                    const personCount = parseInt(this.getAttribute('personcount'));
                    const title = this.getAttribute('title');
                    const image = this.getAttribute('image');
                    const successPercentage = parseInt(this.getAttribute('successpercentage'));

                    // Find and replace placeholders in the template
                    const contentClone = template.content.cloneNode(true);
                    contentClone.querySelector('.cardtitle').textContent = title;
                    contentClone.querySelector('.image').setAttribute('src', "../" + image);
                    contentClone.querySelector('.personCount').textContent = personCount + " personen";
                    contentClone.querySelector('.line-text').textContent = successPercentage + '% slagingspercentage';
                    contentClone.querySelector('.progress-line__bar').style.width = successPercentage + '%';
                    //set the price id
                    contentClone.querySelector('.price').setAttribute('id', 'price' + personCount);

                    const priceElement = contentClone.querySelector('.price');

                    // Create SignalR connection
                    const connection = new signalR.HubConnectionBuilder().withUrl("/signalHub").build();

                    // Start the connection
                    connection.start()
                        .then(() => {
                            // Connection established, now invoke GetPrice method
                            connection.invoke("GetPrice", personCount)
                                .catch(error => console.error(error.toString()));
                        })
                        .catch(error => console.error(error.toString()));

                    // Listen for ReceivePrice event
                    connection.on("ReceivePrice", function (price) {
                        // This handler will be invoked when the server sends the price
                        console.log("Received price:", price);
                        if (priceElement) {
                            priceElement.textContent = price;
                            console.log("Price updated." + el);
                        } else {
                            console.error("Element not found.");
                        }
                    });


                    connection.on("PriceUpdated", function (updatedPrice) {
                        console.log("Price updated:", updatedPrice);

                        // Extract relevant properties
                        const groupSize = updatedPrice.groupSize;
                        const price = updatedPrice.price;
                        //check if the price is for the current person count
                        if (groupSize !== personCount) {
                            return;
                        }

                        if (priceElement) {
                            priceElement.textContent = price;
                        } else {
                            console.error("Element not found.");
                        }


                        // Perform actions here when price is updated, such as updating the UI with the new price for the specified group size
                    });




                    // Add click event listener to navigate to the reservation page
                    contentClone.querySelector('.link').addEventListener('click', () => {
                        fetch("/api/reservation/personCount", {
                            method: "POST",
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify({ PersonCount: personCount })
                        })
                            .then(response => response.json())
                            .then(data => {
                                if (data.status === "success") {
                                    window.location.href = "/reservation/dateselection?" + data.link;
                                }
                            })
                            .catch(error => console.error('Error:', error));
                    });

                    // Append the cloned content to shadow DOM
                    const shadowRoot = this.attachShadow({ mode: 'open' });
                    shadowRoot.appendChild(contentClone);
                }
            }

            // Define the custom element
            customElements.define('person-card', PersonCard);

            // Remove attributes and class from person-card elements
            document.querySelectorAll('.person-card').forEach(personCard => {
                customElements.upgrade(personCard);
                personCard.removeAttribute('title');
                personCard.removeAttribute('image');
                personCard.removeAttribute('personcount');
                personCard.removeAttribute('successpercentage');
                personCard.removeAttribute('price');
                personCard.classList.remove('person-card');
            });
        })
        .catch(error => console.error('Error fetching HTML file:', error));
} catch (error) {
    console.error('Error fetching HTML file:', error);
}
