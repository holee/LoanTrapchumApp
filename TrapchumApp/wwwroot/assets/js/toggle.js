window.toggleInputType = (id, newType) => {
    const input = document.getElementById(id);
    if (input) {
        input.type = newType;
    }
};

window.getElementRect = (el) => {
    const r = el.getBoundingClientRect();
    return {
        bottom: r.bottom + 6,
        right: window.innerWidth - r.right
    };
};
