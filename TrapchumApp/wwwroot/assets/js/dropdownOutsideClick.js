window.dropdownOutsideClick = {
    _handlers: new WeakMap(),

    register: function (element, dotnetHelper) {
        const handler = function (event) {
            if (!element.contains(event.target)) {
                dotnetHelper.invokeMethodAsync('CloseDropdown');
            }
        };

        this._handlers.set(element, handler);
        document.addEventListener('click', handler);
    },

    unregister: function (element) {
        const handler = this._handlers.get(element);
        if (handler) {
            document.removeEventListener('click', handler);
            this._handlers.delete(element);
        }
    }
};
