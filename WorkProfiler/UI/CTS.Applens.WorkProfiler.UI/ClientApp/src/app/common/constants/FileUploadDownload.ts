// Copyright (c) Applens. All Rights Reserved.
export class FileUploadDownload {

  static appendFileToFormData(event) {
    const inputFiles = [];
    if (event !== '') {
      for (let i = 0; i < event.target.files.length; i++) {
        inputFiles.push(event.target.files[i]);
      }
    }
    const formData = new FormData();
    const fileconcatname = "file";
    for (let i = 0; i < inputFiles.length; i++) {
      formData.append(fileconcatname + i.toString(), inputFiles[i]);
    }
    return formData;
  }
  static downloadFileFromBlob(data: BlobPart, downloadFileNameAs: string, fileType: string) {
    const atag = "a";
    const x = new Blob()
    const blob = new Blob([data], { type: fileType });
    const url = window.URL.createObjectURL(blob);
    const downloadTag = document.createElement(atag);
    downloadTag.download = downloadFileNameAs;
    downloadTag.href = url;
    downloadTag.click();
  }

}
