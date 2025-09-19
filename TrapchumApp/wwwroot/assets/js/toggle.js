window.toggleInputType = (id, newType) => {
    const input = document.getElementById(id);
    if (input) {
        input.type = newType;
    }
};
