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

/**
 * Scrolls the highlighted option into view within the results container.
 * @param {string} containerId - The container element ID.
 * @param {number} index - The 0-based index of the highlighted option.
 */
export function scrollToHighlighted(containerId, index) {
    const container = document.getElementById(containerId);
    if (!container) return;

    const results = container.querySelector('.blab-select2__results');
    if (!results) return;

    const options = results.querySelectorAll('.blab-select2__option');
    if (index < 0 || index >= options.length) return;

    const option = options[index];
    const optionTop = option.offsetTop;
    const optionBottom = optionTop + option.offsetHeight;
    const scrollTop = results.scrollTop;
    const visibleBottom = scrollTop + results.clientHeight;

    if (optionTop < scrollTop) {
        results.scrollTop = optionTop;
    } else if (optionBottom > visibleBottom) {
        results.scrollTop = optionBottom - results.clientHeight;
    }
}
