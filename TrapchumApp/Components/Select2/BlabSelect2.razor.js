const handlers = {};

export function init(containerId, dotNetRef) {
    const handler = (e) => {
        const el = document.getElementById(containerId);
        if (el && !el.contains(e.target)) {
            dotNetRef.invokeMethodAsync('OnClickOutside');
        }
    };
    document.addEventListener('mousedown', handler);
    handlers[containerId] = handler;
}

export function destroy(containerId) {
    const handler = handlers[containerId];
    if (handler) {
        document.removeEventListener('mousedown', handler);
        delete handlers[containerId];
    }
}
