var slider = document.getElementById("sidebarprice");
var output = document.getElementById("sidebarpricevalu");
output.innerHTML = slider.value;
slider.oninput = function() {
    output.innerHTML = this.value;
}