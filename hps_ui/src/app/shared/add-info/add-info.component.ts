import { HttpEventType, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ServiceService } from 'src/app/Services/service.service';

@Component({
  selector: 'app-add-info',
  templateUrl: './add-info.component.html',
  styleUrls: ['./add-info.component.scss']
})
export class AddInfoComponent {

  file!: File;
  constructor(private fb: FormBuilder, private s: ServiceService) { }

  form: FormGroup = this.fb.group({
    name: [null, [Validators.required]],
    country: [null, [Validators.required]],
    city: [null, [Validators.required]],
    languages: [null, [Validators.required]],
    dateofbirth: [null, [Validators.required]],
  });

  // On file Select
  onChange(event: any) {
    this.file = event.target.files[0];
    this.form.patchValue({ resume: this.file });
  }

  submitAction() {
    if (this.form.valid) {
      this.s.insertInfo2(this.form.value, this.file).subscribe();
    }
  }

}
