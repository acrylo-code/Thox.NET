

let timeslotsPerDay = [];
const months = ["Januari", "Februari", "Maart", "April", "Mei", "Juni", "Juli",
    "Augustus", "September", "Oktober", "November", "December"];

setupcalendar_item(document);
function setupcalendar_item(document) {
    const daysTag = document.querySelector(".days"),
        currentDate = document.querySelector(".current-date"),
        prevNextIcon = document.querySelectorAll(".calendar_nav_icons span");
    let date = new Date(),
        currYear = date.getFullYear(),
        currMonth = date.getMonth();



    const rendercalendar_item = () => {
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
        let liTags = ""; 

        const handleClick = (dateAttribute, canMoveToPrevMonth) => {
            if (dateAttribute < new Date(currYear, currMonth, 2) && canMoveToPrevMonth) {
                currMonth--;
                if (currMonth < 0) {
                    currMonth = 11;
                    currYear--;
                }
                rendercalendar_item();
            } else if (dateAttribute > new Date(currYear, currMonth + 1, 1)) {
                currMonth++;
                if (currMonth > 11) {
                    currMonth = 0;
                    currYear++;
                }
                rendercalendar_item();
            }
            document.querySelectorAll('.day').forEach(day => {
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
            const day = new Date(currYear, currMonth - 1, lastDateofLastMonth - i + 2);
            liTags += `<li class="${beforeToday}"><a class="day" date="${day.toISOString()}">${lastDateofLastMonth - i + 1}</a></li>`;
        }

        //render the current month days
        for (let i = 1; i <= lastDateofMonth; i++) {
            let isToday = i === date.getDate() && currMonth === new Date().getMonth() && currYear === new Date().getFullYear() ? "active" : "";
            let beforeToday = getDayState(i, 0);
            const day = new Date(currYear, currMonth, i + 1);
            liTags += `<li class="${isToday} ${beforeToday}"><a class="day" date="${day.toISOString()}">${i}</a></li>`;
        }

        //render the first days of the next month
        for (let i = lastDayofMonth; i < 6; i++) {
            let beforeToday = getDayState(i, 1);
            const day = new Date(currYear, currMonth + 1, i - lastDayofMonth + 2);
            liTags += `<li class="inactive ${beforeToday}"><a class="day" date="${day.toISOString()}">${i - lastDayofMonth + 1}</a></li>`;
        }

        daysTag.innerHTML = liTags;
        const days = document.querySelectorAll(".day");
        let activeDayDates = [];
        days.forEach(day => {
            const dateAttribute = new Date(day.getAttribute('date'));
            const yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            if (dateAttribute >= yesterday) {
                day.addEventListener('click', () => {
                    handleClick(dateAttribute, !day.parentElement.classList.contains('disabled'));
                    handleDateChange(dateAttribute);
                });
                activeDayDates.push(dateAttribute);
            }
        });
        if (activeDayDates.length > 0) {
            activeDayDates.sort((a, b) => a - b);
            getReservationSlots(activeDayDates);
        }

        currentDate.innerText = `${months[currMonth]} ${currYear}`;
        var prevButton = document.getElementById("prev");
        prevButton.style.color = (currYear < new Date().getFullYear() ||
            (currYear === new Date().getFullYear() && currMonth <= new Date().getMonth())) ?
            "#686868" : "#ff8800";
    }
    rendercalendar_item();

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
            rendercalendar_item();
        });
    });
}

function getReservationSlots(dates) {
    const dateStrings = dates.map(date => date.toISOString());

    fetch('/api/reservation/getTimeSlots', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Dates: dateStrings })
    })
        .then(response => response.json())
        .then(data => {
            if (data.status === "success") {
                timeslotsPerDay = data.data;
                const days = document.querySelectorAll(".day");
                
                days.forEach(day => {
                    
                    const dateAttribute = new Date(day.getAttribute('date'));
                    const today = new Date();
                    if (dateAttribute >= today) {
                        createcalendar_nav_icons(dateAttribute, day);
                    }
                });
            }
        })
}

function createcalendar_nav_icons(dateAttribute, day) {

    const date = new Date(dateAttribute);
    const dateIsoString = date.toISOString().split('T')[0];

    let openSlotFound = false;
    let closedSlotFound = false;
    timeslotsPerDay.forEach(slot => {
        if (slot.reservationDate && slot.reservationDate.split('T')[0] === dateIsoString) {
            if (slot.availabilityState == 0) {
                openSlotFound = true;
            } else if (slot.availabilityState == 1) {
                closedSlotFound = true;
            }
        }
    });

    if (openSlotFound) {
        const icon = document.createElement("i");
        icon.classList.add("fas", "fa-circle-check", "date-icon", "date-icon-open");
        day.appendChild(icon);
    } else if (closedSlotFound) {
        const icon = document.createElement("i");
        icon.classList.add("fas", "fa-circle-minus", "date-icon", "date-icon-closed");
        day.appendChild(icon);
    }
}



handleDateChange = (dateAttribute) => {
    const date = new Date(dateAttribute);
    const dateIsoString = date.toISOString().split('T')[0];

    const timestamps = timeslotsPerDay.filter(slot => {
        if (slot.reservationDate)
            return slot.reservationDate.split('T')[0] === dateIsoString;
    });
    let title = document.querySelector("#time_selector_title");
    title.innerHTML = `<a class="orange">
        ${date.getUTCDate()} 
        ${months[date.getUTCMonth()]} 
        ${date.getUTCFullYear()}`;

    document.querySelectorAll('#time_selector').forEach(element => {
        element.style.display = 'block';
    });

    if (timestamps.length === 0) {
        document.querySelectorAll('#no_date').forEach(element => {
            element.style.display = 'block';
        });
    } else {
        document.querySelectorAll('#no_date').forEach(element => {
            element.style.display = 'none';
        });
    }
    const container = document.getElementById('timeSlotsContainer');
    container.innerHTML = '';
    timestamps.forEach(timestamp => {
        CreateTimeSlotElement(timestamp, container);
    });
}


CreateTimeSlotElement = (timestamp, container) => {
    const timeSlotElement = document.createElement('div');
    timeSlotElement.classList.add('time_slot');
    timeSlotElement.textContent = new Date(timestamp.reservationDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    timeSlotElement.setAttribute('timestamp', JSON.stringify(timestamp.reservationDate));
    if (timestamp.availabilityState !== 0) {
        timeSlotElement.classList.add('disabled');
    }
    const personCount = getQueryParameter('personCount');

    timeSlotElement.addEventListener('click', () => {
        let encodedDate = encodeURIComponent(timestamp.reservationDate);
        console.log(`Clicked - Person Count: ${personCount}, Reservation Date: ${encodedDate}`);
        let url = `/reservation/PersonalDetails?personCount=${personCount}&reservationDate=${encodedDate}`;
        window.location.href = url;
    });

    container.appendChild(timeSlotElement);
}
function getQueryParameter(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}




//// Start the connection
//connection.start()
//    .then(() => console.log('SignalR Connected.'))
//    .catch(error => console.error(error.toString()));

//connection.on("ReservationSlotUpdate", function (updatedSlot) {
//    console.log("ReservationSlot updated:", updatedSlot);
//    const shadowRoot = document.querySelector("mijn-element").shadowRoot;
//    const container = shadowRoot.getElementById('timeSlotsContainer');
//    let timestamp = {};
//    timestamp.AvailabilityState = updatedSlot.entity.state == 0 ? "available" : "unavailable";
//    timestamp.ReservationDate = updatedSlot.entity.reservationDate;

//    switch (updatedSlot.state) {
//        case 0:
//            console.log("Slot is Detached.");
//            break;
//        case 1:
//            console.log("Slot is Unchanged.");
//            break;
//        case 2:
//            console.log("Slot is reserved.");
//            DeleteTimeSlotElement(timestamp, container);
//            break;
//        case 3:
//            console.log("Slot is Modified.");
//            if (UserClickedSlot) {
//                window.location.href = "/reservation/reservationComplete";
//            }
//            UpdateTimeSlotElement(timestamp, container);
//            break;
//        case 4:
//            console.log("Slot is Added.");
//            CreateTimeSlotElement(timestamp, container);
//            break;
//        default:
//            console.error("Unknown state.");
//            break;
//    }
//});


//DeleteTimeSlotElement = (timestampToDelete, container) => {
//    container.querySelectorAll('.time_slot').forEach(timeSlot => {
//        let currentTimestamp = JSON.parse(timeSlot.getAttribute('timestamp'));
//        let timestampUTC = new Date(currentTimestamp); // Convert to UTC
//        //convert the updatedDate to a date object
//        let updatedDateUTC = new Date(timestampToDelete.ReservationDate); // Convert to UTC
//        // Compare timestamps to find matching time slots
//        if (updatedDateUTC.getTime() === timestampUTC.getTime()) {
//            timeSlot.remove(); // Corrected from Slot.remove()
//        }
//    });
//}


////handleTimeSlotUpdate = (date) => {
//UpdateTimeSlotElement = (timestamp, container) => {
//    // Loop over all time slots and update the availability
//    let timeSlotFound = false;
//    container.querySelectorAll('.time_slot').forEach(timeSlot => {
//        let currentTimestamp = JSON.parse(timeSlot.getAttribute('timestamp'));
//        let timestampUTC = new Date(currentTimestamp); // Convert to UTC
//        //convert the updatedDate to a date object
//        let updatedDateUTC = new Date(timestamp.ReservationDate); // Convert to UTC
//        // Compare timestamps to find matching time slots
//        if (updatedDateUTC.getTime() === timestampUTC.getTime()) {
//            timeSlotFound = true;
//            if (timestamp.AvailabilityState == "available") {
//                timeSlot.classList.remove('disabled');
//            } else {
//                timeSlot.classList.add('disabled');
//            }
//        }
//    });
//}








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


//const connection = new signalR.HubConnectionBuilder().withUrl("/signalHub").build();

//try {
//    fetch('../../Components/Calender/Calender_Element.html')
//        .then(response => response.text())
//        .then(html => {
//            template.innerHTML = html;

//            class MijnElement extends HTMLElement {
//                constructor() {
//                    super();
//                }
//                connectedCallback() {
//                    const shadowRoot = this.attachShadow({ mode: 'open' });
//                    shadowRoot.appendChild(template.content.cloneNode(true));
//                    setupcalendar_item(shadowRoot);
//                    //get the personCount out of the custom tag
//                    personCount = this.getAttribute('personCount');
//                    let title = shadowRoot.querySelector("#time_selector_title");
//                    title.innerHTML = `Selecteer een tijdslot voor < a class="orange" > ${ personCount }</a > personen`;
//                    handleDateChange(new Date().getTime());
//                }
//            }

//            // Define the custom element
//            customElements.define('mijn-element', MijnElement);
//        })
//        .catch(error => console.error('Error fetching HTML file:', error));
//} catch (error) {
//    console.error('Error fetching HTML file:', error);
//}