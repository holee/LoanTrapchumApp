// myTableDrag.js – handles mouseup outside the table to end drag selection
const _handlers = new Map();

export function init(tableId, dotNetRef) {
    const onMouseUp = () => {
        dotNetRef.invokeMethodAsync('EndDragFromJs');
    };
    document.addEventListener('mouseup', onMouseUp);
    _handlers.set(tableId, onMouseUp);
}

export function dispose(tableId) {
    const handler = _handlers.get(tableId);
    if (handler) {
        document.removeEventListener('mouseup', handler);
        _handlers.delete(tableId);
    }
}
