import { Component, OnInit, Inject } from '@angular/core';
import { DocumentModel } from '../shared/models/document.model';
import { DocumentService } from '../services/document.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DialogComponent } from './dialog/dialog.component';
import {saveAs} from 'file-saver';
import { environment } from '../../environments/environment.prod';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss']
})
export class DocumentsComponent implements OnInit {
  title: 'Documents';
  documents: DocumentModel[] = [];

  documentInfo: FormGroup = new FormGroup({
  });

  constructor(
    private documentService: DocumentService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.getDocuments();
  }

  getDocuments() {
    this.documentService.getAll().subscribe(response => {
      this.documents = response;
    });
  }

  createDocument() {
    // this.documentService.create(this.documentInfo.value).subscribe(result => {
    //   this.documentInfo.reset();
    //   this.documentInfo.setErrors(null);
    // });
  }

  remove(id: number) {
    this.documentService.deleteDocument(id).subscribe(result => {
        this.documents = this.documents.filter(elm => elm.id !== id);
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '350px',
    });
    dialogRef.beforeClose().subscribe(result => {
        this.documentInfo = result;
        if (this.documentInfo.valid === true) {
          this.createDocument();
        }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getDocuments();
    });
  }

  save(id: number, name: string) {
    this.documentService.saveFile(id).subscribe(response => {
    const blob = new Blob([response]);
        saveAs(blob, name);
    });
  }

  private saveToFileSystem(response) {
    console.log(response);
    }

}


