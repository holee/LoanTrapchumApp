// BlabGridView – JS support module
// Provides: CSV export, clipboard copy, print, column resize

/**
 * Generates a CSV file and triggers a browser download.
 * @param {string[]} headers - Column header titles.
 * @param {string[][]} rows - 2D array of cell values.
 * @param {string} filename - File name without extension.
 */
export function exportCsv(headers, rows, filename) {
    const escapeCsv = (val) => {
        if (val == null) return '';
        const str = String(val);
        if (str.includes(',') || str.includes('"') || str.includes('\n') || str.includes('\r')) {
            return '"' + str.replace(/"/g, '""') + '"';
        }
        return str;
    };

    const lines = [];
    lines.push(headers.map(escapeCsv).join(','));
    for (const row of rows) {
        lines.push(row.map(escapeCsv).join(','));
    }

    const csv = '\uFEFF' + lines.join('\r\n'); // BOM for Excel UTF-8
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = (filename || 'export') + '.csv';
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();

    setTimeout(() => {
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }, 100);
}

/**
 * Copies text to the clipboard.
 * @param {string} text - Tab-separated text to copy.
 */
export async function copyToClipboard(text) {
    try {
        await navigator.clipboard.writeText(text);
        showToast('Copied to clipboard!');
    } catch {
        // Fallback for older browsers
        const textarea = document.createElement('textarea');
        textarea.value = text;
        textarea.style.cssText = 'position:fixed;left:-9999px;top:-9999px;';
        document.body.appendChild(textarea);
        textarea.select();
        try {
            document.execCommand('copy');
            showToast('Copied to clipboard!');
        } catch {
            showToast('Copy failed', true);
        }
        document.body.removeChild(textarea);
    }
}

/**
 * Opens a print-friendly window with the table data.
 * @param {string[]} headers - Column header titles.
 * @param {string[][]} rows - 2D array of cell values.
 */
export function printTable(headers, rows) {
    const escapeHtml = (val) => {
        if (val == null) return '';
        return String(val)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    };

    let tableHtml = '<table><thead><tr>';
    for (const h of headers) {
        tableHtml += `<th>${escapeHtml(h)}</th>`;
    }
    tableHtml += '</tr></thead><tbody>';
    for (const row of rows) {
        tableHtml += '<tr>';
        for (const cell of row) {
            tableHtml += `<td>${escapeHtml(cell)}</td>`;
        }
        tableHtml += '</tr>';
    }
    tableHtml += '</tbody></table>';

    const html = `<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Print Table</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: "Segoe UI", -apple-system, Arial, sans-serif; font-size: 12px; color: #1e293b; padding: 20px; }
        table { width: 100%; border-collapse: collapse; border: 1px solid #cbd5e1; }
        th { background: #f1f5f9; border: 1px solid #cbd5e1; padding: 8px 12px; font-weight: 600; font-size: 11px; color: #475569; text-transform: uppercase; letter-spacing: 0.03em; text-align: left; }
        td { border: 1px solid #e2e8f0; padding: 6px 12px; font-size: 12px; color: #334155; }
        tr:nth-child(even) { background: #f8fafc; }
        @media print { body { padding: 0; } }
    </style>
</head>
<body>
    ${tableHtml}
    <script>window.onload = function() { window.print(); window.onafterprint = function() { window.close(); }; };<\/script>
</body>
</html>`;

    const printWindow = window.open('', '_blank', 'width=900,height=600');
    if (printWindow) {
        printWindow.document.write(html);
        printWindow.document.close();
    }
}

/**
 * Initializes column resize handlers for a grid.
 * @param {string} gridId - The grid container element ID.
 */
export function initColumnResize(gridId) {
    const grid = document.getElementById(gridId);
    if (!grid) return;

    const handles = grid.querySelectorAll('.bgv__resize-handle');

    handles.forEach(handle => {
        handle.addEventListener('mousedown', (e) => {
            e.preventDefault();
            e.stopPropagation();

            const th = handle.parentElement;
            const startX = e.clientX;
            const startWidth = th.offsetWidth;

            const onMouseMove = (moveEvent) => {
                const delta = moveEvent.clientX - startX;
                const newWidth = Math.max(50, startWidth + delta);
                th.style.width = newWidth + 'px';
                th.style.minWidth = newWidth + 'px';
            };

            const onMouseUp = () => {
                document.removeEventListener('mousemove', onMouseMove);
                document.removeEventListener('mouseup', onMouseUp);
                document.body.style.cursor = '';
                document.body.style.userSelect = '';
            };

            document.addEventListener('mousemove', onMouseMove);
            document.addEventListener('mouseup', onMouseUp);
            document.body.style.cursor = 'col-resize';
            document.body.style.userSelect = 'none';
        });
    });
}

// ── Internal helpers ──────────────────────────────────────

function showToast(message, isError = false) {
    const toast = document.createElement('div');
    toast.textContent = message;
    toast.style.cssText = `
        position: fixed;
        bottom: 24px;
        left: 50%;
        transform: translateX(-50%) translateY(10px);
        padding: 10px 20px;
        background: ${isError ? '#ef4444' : '#1e293b'};
        color: #fff;
        border-radius: 8px;
        font-size: 13px;
        font-weight: 500;
        font-family: "Segoe UI", -apple-system, Arial, sans-serif;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        z-index: 99999;
        opacity: 0;
        transition: opacity 0.2s, transform 0.2s;
        pointer-events: none;
    `;

    document.body.appendChild(toast);

    requestAnimationFrame(() => {
        toast.style.opacity = '1';
        toast.style.transform = 'translateX(-50%) translateY(0)';
    });

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(-50%) translateY(10px)';
        setTimeout(() => {
            if (toast.parentNode) toast.parentNode.removeChild(toast);
        }, 200);
    }, 2000);
}
