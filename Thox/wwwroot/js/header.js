
//function ToggleDropdown() {
//    const dropdownContent = document.getElementById("header-menu-dropdown");
//    // Toggle the visibility of the dropdown
//    cloneBigMenuItems();
//    dropdownContent.classList.toggle("show");
//}

//// Close the dropdown if the user clicks outside of it
//window.onclick = function (e) {
//	if (!e.target.matches('.header-menu-dropdown-button')) {
//		var myDropdown = document.getElementById("header-menu-dropdown");
//		if (myDropdown.classList.contains('show')) {
//			myDropdown.classList.remove('show');
//		}
//	}

//	if (!e.target.matches('.header-menu-side-bar') && !e.target.matches('#check') && !e.target.matches('#header-menu-burger') && !e.target.matches('.header-menu-burger__line')) {
//		var checkbox = document.getElementById("check");
//		checkbox.checked = false;
//	}
//}

//function cloneBigMenuItems() {
//    // Select all elements with the class header-menu__big
//    var bigMenuItems = document.querySelectorAll('.header-menu__big');

//    // Select the dropdown menu where we want to append the links
//    var dropdownMenu = document.getElementById('header-menu-dropdown');

//    while (dropdownMenu.firstChild) {
//        dropdownMenu.removeChild(dropdownMenu.firstChild);
//    }

//    // Loop through each big menu item
//    bigMenuItems.forEach(function (item) {
//        // Clone the big menu item
//        var clonedItem = item.firstChild.cloneNode(true);

//        // Append the cloned item to the dropdown menu
//        dropdownMenu.appendChild(clonedItem);
//    });
//}


//// Call the function when the document is fully loaded
//document.addEventListener('DOMContentLoaded', function () {
//    cloneBigMenuItems();
//});