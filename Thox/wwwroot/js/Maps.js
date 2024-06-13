var map, directionsService, directionsRenderer;
    async function initMap() {
    // Set dark mode style
    var darkMode = [
      {
        elementType: "geometry",
        stylers: [{ color: "#212121" }],
      },
      {
        elementType: "labels.icon",
        stylers: [{ visibility: "off" }],
      },
      {
        elementType: "labels.text.fill",
        stylers: [{ color: "#757575" }],
      },
      {
        elementType: "labels.text.stroke",
        stylers: [{ color: "#212121" }],
      },
      {
        featureType: "administrative",
        elementType: "geometry",
        stylers: [{ color: "#757575" }],
      },
      {
        featureType: "administrative.country",
        elementType: "labels.text.fill",
        stylers: [{ color: "#9e9e9e" }],
      },
      {
        featureType: "administrative.land_parcel",
        stylers: [{ visibility: "off" }],
      },
      {
        featureType: "administrative.locality",
        elementType: "labels.text.fill",
        stylers: [{ color: "#bdbdbd" }],
      },
      {
        featureType: "poi",
        elementType: "labels.text.fill",
        stylers: [{ color: "#757575" }],
      },
      {
        featureType: "poi.park",
        elementType: "geometry",
        stylers: [{ color: "#181818" }],
      },
      {
        featureType: "poi.park",
        elementType: "labels.text.fill",
        stylers: [{ color: "#616161" }],
      },
      {
        featureType: "poi.park",
        elementType: "labels.text.stroke",
        stylers: [{ color: "#1b1b1b" }],
      },
      {
        featureType: "road",
        elementType: "geometry.fill",
        stylers: [{ color: "#5a4230" }],
      },
      {
        featureType: "road",
        elementType: "labels.text.fill",
        stylers: [{ color: "#82522d" }],
      },
      {
        featureType: "road.arterial",
        elementType: "geometry",
        stylers: [{ color: "#aa6427" }],
      },
      {
        featureType: "road.highway",
        elementType: "geometry",
        stylers: [{ color: "#D4761C" }],
      },
      {
        featureType: "road.highway.controlled_access",
        elementType: "geometry",
        stylers: [{ color: "#ff8800" }],
      },
      {
        featureType: "road.local",
        elementType: "labels.text.fill",
        stylers: [{ color: "#616161" }],
      },
      {
        featureType: "transit",
        elementType: "labels.text.fill",
        stylers: [{ color: "#757575" }],
      },
      {
        featureType: "water",
        elementType: "geometry",
        stylers: [{ color: "#000000" }],
      },
      {
        featureType: "water",
        elementType: "labels.text.fill",
        stylers: [{ color: "#3d3d3d" }],
        }
    ];
    // Create map object
    map = new google.maps.Map(document.getElementById("map"), {
      center: { lat: 52.61841006936597, lng: 6.041329898176236 },
      zoom: 14,
      styles: darkMode, // Apply dark mode style
	  zoomControl: true, // Show the default zoom control
    mapTypeControl: false, // Show the default map type control
    streetViewControl: true, // Show the default street view control
    fullscreenControl: true, // Show the default fullscreen control
    zoomControlOptions: {
      position: google.maps.ControlPosition.RIGHT_TOP, // Position the zoom control on the top right
    },
    mapTypeControlOptions: {
      position: google.maps.ControlPosition.TOP_LEFT, // Position the map type control on the top left
      style: google.maps.MapTypeControlStyle.DROPDOWN_MENU, // Style the map type control as a dropdown menu
    },
    streetViewControlOptions: {
      position: google.maps.ControlPosition.RIGHT_BOTTOM, // Position the street view control on the bottom right
    },
    fullscreenControlOptions: {
      position: google.maps.ControlPosition.RIGHT_TOP, // Position the fullscreen control on the top right
    },
    });

	var marker = new google.maps.Marker({
    position: { lat: 52.61841006936597, lng: 6.041329898176236 },
    map: map,
    icon: {
      url: "images/googleMaps/escape-marker.png", // Path to your custom marker image
      scaledSize: new google.maps.Size(60, 60), // Adjust the size of the marker
      origin: new google.maps.Point(0, 0), // Marker image origin
      anchor: new google.maps.Point(30, 60) // Anchor point of the marker
    }
    });

      var childDiv = document.getElementById('map').firstElementChild;
      childDiv.style.backgroundColor = '#212121';
}