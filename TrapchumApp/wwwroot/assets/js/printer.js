window.downloadFile = function (fileName, contentType, base64Data) {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = `data:${contentType};base64,${base64Data}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};


function printPage() {
    window.print();
}



window.printPdf = (pdfDataUrl) => {
    // Create an iframe to load the PDF
    const iframe = document.createElement('iframe');

    iframe.style.display = 'none'; // Hide the iframe

    document.body.appendChild(iframe);

    // Set the PDF data URL as the iframe source
    iframe.src = pdfDataUrl;

    // Wait for the iframe to load, then trigger print
    iframe.onload = () => {
        try {
            iframe.contentWindow.print();
        } catch (e) {
            console.error('Error printing PDF:', e);
        }

        // Clean up: remove the iframe after printing
        setTimeout(() => {
            document.body.removeChild(iframe);
        }, 1000); // Delay to ensure print dialog appears
    };
};

window.printPdfFromUrl = (url) => {
    //const iframe = document.createElement('iframe');
    //iframe.style.display = 'none';
    //document.body.appendChild(iframe);
    //iframe.src = url;
    //iframe.onload = () => {
    //    try {
    //        iframe.contentWindow.print();
    //    } catch (e) {
    //        console.error('Error printing PDF:', e);
    //    }
    //    setTimeout(() => {
    //        document.body.removeChild(iframe);
    //    }, 1000);
    //};

    const printWindow = window.open(url, '_blank');
    printWindow.focus();
    printWindow.onload = () => {
        printWindow.print();
    };

};