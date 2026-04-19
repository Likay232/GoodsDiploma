// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

async function downloadExcelGeneric(url, data, fileName) {

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    if (!response.ok) {
        alert("Ошибка при формировании файла");
        return;
    }

    const blob = await response.blob();
    const urlBlob = window.URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = urlBlob;
    a.download = fileName;

    document.body.appendChild(a);
    a.click();

    a.remove();
    window.URL.revokeObjectURL(urlBlob);
}