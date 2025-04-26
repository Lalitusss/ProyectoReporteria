function downloadFileFromUrl(url) {
    var link = document.createElement('a');
    link.href = url;
    link.download = 'credencial.pdf'; // Puedes cambiar el nombre del archivo aquí
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
