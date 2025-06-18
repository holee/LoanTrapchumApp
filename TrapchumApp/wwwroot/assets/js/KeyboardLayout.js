window.changeKeyboardLayout = function () {
    // Get the focused input element
    const input = document.querySelector('input:focus');
    if (input) {
        // Example: Set language to a specific locale (e.g., French)
        input.setAttribute('lang', 'en-US');
        // Optionally, set inputmode for specific types of input
        input.setAttribute('inputmode', 'text');

    }
};

//window.initKhmerKeyboard = () => {
//    const input = document.querySelector("#input");

//    const keyboard = new SimpleKeyboard.default({
//        onChange: inputChanged,
//        onKeyPress: buttonPressed,
//        layout: {
//            default: [
//                "៊ ់ ា ិ ី ញ ឫ",
//                "ឋ ថ ណ ត ថ ធ ន",
//                "Enter Space Backspace"
//            ]
//        },
//        display: {
//            Backspace: "⌫",
//            Enter: "⏎",
//            Space: "␣"
//        }
//    });

//    function inputChanged(inputText) {
//        input.value = inputText;
//        input.dispatchEvent(new Event('input')); // Update Blazor binding
//    }

//    function buttonPressed(button) {
//        if (button === "Backspace") {
//            input.value = input.value.slice(0, -1);
//        } else if (button === "Enter") {
//            console.log("Enter pressed");
//        } else if (button === "Space") {
//            input.value += " ";
//        } else {
//            input.value += button;
//        }
//        keyboard.setInput(input.value);
//        input.dispatchEvent(new Event('input')); // Update Blazor binding
//    }

//    input.addEventListener("input", (e) => {
//        keyboard.setInput(e.target.value);
//    });
//};
