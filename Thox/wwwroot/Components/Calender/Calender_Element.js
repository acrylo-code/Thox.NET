
const template = document.createElement('template');
let personCount;
const connection = new signalR.HubConnectionBuilder().withUrl("/signalHub").build();

try {
    fetch('../../Components/Calender/Calender_Element.html')
        .then(response => response.text())
        .then(html => {
            template.innerHTML = html;

            class MijnElement extends HTMLElement {
                constructor() {
                    super();
                }
                connectedCallback() {
                    const shadowRoot = this.attachShadow({ mode: 'open' });
                    shadowRoot.appendChild(template.content.cloneNode(true));
                    setupCalendar(shadowRoot);
                    //get the personCount out of the custom tag
                    personCount = this.getAttribute('personCount');
                    let title = shadowRoot.querySelector("#time_selector_title");
                    title.innerHTML = `Selecteer een tijdslot voor <a class="orange">${personCount}</a> personen`;
                    handleDateChange(new Date().getTime()); 
                }
            }

            // Define the custom element
            customElements.define('mijn-element', MijnElement);
        })
        .catch(error => console.error('Error fetching HTML file:', error));
} catch (error) {
    console.error('Error fetching HTML file:', error);
}



function setupCalendar(document) {
    const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");
    // getting new date, current year and month
    let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();

    const months = ["Januari", "Februari", "Maart", "April", "Mei", "Juni", "Juli",
    "Augustus", "September", "Oktober", "November", "December"];

    const renderCalendar = () => {
        const getDayState = (i, dayOffset, monthOffset = 0, elseState = "") => {
            if (currYear < new Date().getFullYear() ||
                (currYear === new Date().getFullYear() && currMonth + monthOffset < new Date().getMonth()) ||
                (currYear === new Date().getFullYear() && currMonth + dayOffset === new Date().getMonth() && i < date.getDate())) {
                return "disabled";
            } else {
                return elseState;
            }
        };
        let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(), 
            lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(), 
            lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(), 
            lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate(); 
        let liTags = ""; // Changed to liTags to hold multiple li elements
    
        // Function to handle click event
        const handleClick = (dateAttribute, canMoveToPrevMonth) => {
                //if the selected day is the next month or previous month, change the month
                if (dateAttribute < new Date(currYear, currMonth, 1) && canMoveToPrevMonth) {
                    currMonth--;
                    if (currMonth < 0) {
                        currMonth = 11;
                        currYear--;
                    }
                } else if (dateAttribute > new Date(currYear, currMonth + 1, 0)) {
                    currMonth++;
                    if (currMonth > 11) {
                        currMonth = 0;
                        currYear++;
                    }  
                }
                renderCalendar();
                //if the date is not disabled and the date is in the future
                //find the day that was clicked based on the date attribute and add the selected class
                document.querySelectorAll('#day').forEach(day => {
                    if (day.getAttribute('date') == dateAttribute.toISOString() && !day.parentElement.classList.contains('disabled')) {
                        day.parentElement.classList.add('selected');
                    } else {
                        day.parentElement.classList.remove('selected');
                    }
                });  

        };
    
        //render the last days of the last month
        for (let i = firstDayofMonth; i > 0; i--) {
            let beforeToday = getDayState(i, 1, -1, "inactive");
            const day = new Date(currYear, currMonth - 1, lastDateofLastMonth - i + 1);
            liTags += `<li class="${beforeToday}"><a id="day" date="${day.toISOString()}">${lastDateofLastMonth - i + 1}</a></li>`;
        } 
    
        //render the current month days
        for (let i = 1; i <= lastDateofMonth; i++) {
            let isToday = i === date.getDate() && currMonth === new Date().getMonth() && currYear === new Date().getFullYear() ? "active" : "";
            let beforeToday = getDayState(i, 0);
            const day = new Date(currYear, currMonth, i);
            liTags += `<li class="${isToday} ${beforeToday}"><a id="day" date="${day.toISOString()}">${i}</a></li>`;
        }
    
        //render the first days of the next month
        for (let i = lastDayofMonth; i < 6; i++) {
            let beforeToday = getDayState(i, 1);
            const day = new Date(currYear, currMonth + 1, i - lastDayofMonth + 1);
            liTags += `<li class="inactive ${beforeToday}"><a id="day" date="${day.toISOString()}">${i - lastDayofMonth + 1}</a></li>`;
        }
    
        // Set the content of daysTag to liTags
        daysTag.innerHTML = liTags;
    
        // Add event listeners to each day <a> tag
        const days = document.querySelectorAll("#day");
        days.forEach(day => {
            day.addEventListener('click', () => {
                const dateAttribute = day.getAttribute('date');
                handleClick(new Date(dateAttribute), !day.parentElement.classList.contains('disabled'));
                handleDateChange(new Date(dateAttribute));
            });
        });
    
        currentDate.innerText = `${months[currMonth]} ${currYear}`;
        var prevButton = document.getElementById("prev");
        prevButton.style.color = (currYear < new Date().getFullYear() ||
            (currYear === new Date().getFullYear() && currMonth <= new Date().getMonth())) ?
            "#686868" : "#ff8800";
    }
    renderCalendar();

    prevNextIcon.forEach(icon => {
        icon.addEventListener("click", () => {
            if (icon.id === "prev" && (currYear < new Date().getFullYear() || (currMonth <= new Date().getMonth() && currYear === new Date().getFullYear()))) return;
            currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
            if (currMonth < 0 || currMonth > 11) {
                date = new Date(currYear, currMonth, new Date().getDate());
                currYear = date.getFullYear();
                currMonth = date.getMonth();
            } else
                date = new Date();
            renderCalendar();
        });
    });
}


handleDateChange = (dateAttribute) => {
    const shadowRoot = document.querySelector("mijn-element").shadowRoot;
    const date = new Date(dateAttribute);
    const year = date.getFullYear();
    const month = date.getMonth() + 1; // Month is zero-based, so add 1
    const day = date.getDate();
    // Fetch the time slots for the selected date
    fetch('/api/reservation/getTimeSlots', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            date: date.toISOString()
        })
    })
        .then(response => response.json()) // Parse the JSON response
        .then(data => {
            // Get the array of timestamps from the data
            const timestamps = JSON.parse(data.data);
            console.log('Time slots:', timestamps);
            let title = shadowRoot.querySelector("#time_selector_title");
            title.innerHTML = `<a class="orange">${year}/${month}/${day}</a> voor <a class="orange">${personCount}</a> personen`;
            //check if timestamps is empty
            if (timestamps.length === 0) {
                console.log('No time slots available for the selected date.');
                //get all elements with id no-data and show them
                shadowRoot.querySelectorAll('#no_date').forEach(element => {
                    element.style.display = 'block';
                });
            } else {
                shadowRoot.querySelectorAll('#no_date').forEach(element => {
                    element.style.display = 'none';
                });
            }
             
            const container = shadowRoot.getElementById('timeSlotsContainer');
            // Clear the container before adding new elements
            container.innerHTML = '';

            // Loop through the timestamps and create a new element for each time slot
            timestamps.forEach(timestamp => {
                CreateTimeSlotElement(timestamp, container);
            });
        })
        .catch(error => {
            console.error('Error fetching time slots:', error);
            // Handle errors appropriately
        });
}


let UserClickedSlot = false;

// Start the connection
connection.start()
    .then(() => console.log('SignalR Connected.'))
    .catch(error => console.error(error.toString()));

connection.on("ReservationSlotUpdate", function (updatedSlot) {
    console.log("ReservationSlot updated:", updatedSlot);
    const shadowRoot = document.querySelector("mijn-element").shadowRoot;
    const container = shadowRoot.getElementById('timeSlotsContainer');
    let timestamp = {};
    timestamp.AvailabilityState = updatedSlot.entity.state == 0 ? "available" : "unavailable";
    timestamp.ReservationDate = updatedSlot.entity.reservationDate;

    switch (updatedSlot.state) {
        case 0:
            console.log("Slot is Detached.");
            break;
        case 1:
            console.log("Slot is Unchanged.");
            break;
        case 2:
            console.log("Slot is reserved.");
            DeleteTimeSlotElement(timestamp, container);
            break;
        case 3:
            console.log("Slot is Modified.");
            if (UserClickedSlot) {
                window.location.href = "/reservation/reservationComplete";
            }
            UpdateTimeSlotElement(timestamp, container);
            break;
        case 4:
            console.log("Slot is Added.");
            CreateTimeSlotElement(timestamp, container);
            break;
        default:
            console.error("Unknown state.");
            break;
    }
});


DeleteTimeSlotElement = (timestampToDelete, container) => {
    container.querySelectorAll('.time_slot').forEach(timeSlot => {
        let currentTimestamp = JSON.parse(timeSlot.getAttribute('timestamp'));
        let timestampUTC = new Date(currentTimestamp); // Convert to UTC
        //convert the updatedDate to a date object
        let updatedDateUTC = new Date(timestampToDelete.ReservationDate); // Convert to UTC
        // Compare timestamps to find matching time slots
        if (updatedDateUTC.getTime() === timestampUTC.getTime()) {
            timeSlot.remove(); // Corrected from Slot.remove()
        }
    });
}


//handleTimeSlotUpdate = (date) => {
UpdateTimeSlotElement = (timestamp, container) => {
    // Loop over all time slots and update the availability
    let timeSlotFound = false;
    container.querySelectorAll('.time_slot').forEach(timeSlot => {
        let currentTimestamp = JSON.parse(timeSlot.getAttribute('timestamp'));
        let timestampUTC = new Date(currentTimestamp); // Convert to UTC
        //convert the updatedDate to a date object
        let updatedDateUTC = new Date(timestamp.ReservationDate); // Convert to UTC
        // Compare timestamps to find matching time slots
        if (updatedDateUTC.getTime() === timestampUTC.getTime()) {
            timeSlotFound = true;
            if (timestamp.AvailabilityState == "available") {
                timeSlot.classList.remove('disabled');
            } else {
                timeSlot.classList.add('disabled');
            }
        }
    });
}


CreateTimeSlotElement = (timestamp, container) => {
    const timeSlotElement = document.createElement('div');
    timeSlotElement.classList.add('time_slot');
    timeSlotElement.textContent = new Date(timestamp.ReservationDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    timeSlotElement.setAttribute('timestamp', JSON.stringify(timestamp.ReservationDate));
    if (timestamp.AvailabilityState !== "available") {
        timeSlotElement.classList.add('disabled');
    }

    timeSlotElement.addEventListener('click', () => {
        UserClickedSlot = true;
        connection.invoke("ReserveTimeSlot", timestamp.ReservationDate)
            .catch(error => console.error('Error reserving time slot:', error));
    });
    // Append the new time slot element to the container
    container.appendChild(timeSlotElement);
}








//    // Listen for ReceivePrice event
//    connection.on("ReceivePrice", function (price) {
//        // This handler will be invoked when the server sends the price
//        console.log("Received price:", price);
//        if (priceElement) {
//            priceElement.textContent = price;
//            console.log("Price updated." + el);
//        } else {
//            console.error("Element not found.");
//        }
//    });


