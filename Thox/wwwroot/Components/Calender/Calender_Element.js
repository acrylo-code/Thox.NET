
const template = document.createElement('template');

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
                    const personCount = this.getAttribute('personCount');
                    let title = shadowRoot.querySelector("#time_selector_title");
                    title.innerHTML = `Selecteer een tijdslot voor <a class="orange">${personCount}</a> personen`;
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


handleDateChange = (date) => {
    //fetch the time slots for the selected date
    fetch('/api/reservation/getTimeSlots', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            date: date.toISOString()
        })
    })
}
