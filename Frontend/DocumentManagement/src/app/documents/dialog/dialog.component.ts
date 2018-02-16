import {Component, Inject, ViewChild} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { DocumentService } from '../../services/document.service';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {
 // @ViewChild('fileInput') fileInput;
  // fileToUpload: File = null;

  formData: FormData = null;

  form: FormGroup = new FormGroup({
    description: new FormControl(null, Validators.required)
  });

  constructor(
    private documentService: DocumentService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  submit(form) {
    // this.handleFileInput(this.fileInput.nativeElement.files);
    this.dialogRef.close(form);
    this.documentService.postFile(this.formData, form).subscribe(result => {
    });
  }

  onFileChange(files) {
    if (files.length === 0) {
    return;
    }
    const formData = new FormData();
    formData.append('uploadFile', files[0]);
    this.formData = formData;

  //  this.form.controls.fileName.value = file[0].fileName;
  }
}
