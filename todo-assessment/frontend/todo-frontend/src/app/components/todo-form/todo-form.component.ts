import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TodoService } from '../../services/todo.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './todo-form.component.html',
  styleUrls: ['./todo-form.component.scss']
})
export class TodoFormComponent implements OnInit {
  @Output() created = new EventEmitter<void>();
  form: FormGroup;
  submitting = false;
  error?: string;

  constructor(private fb: FormBuilder, private todoService: TodoService) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['']
    });
  }
  ngOnInit(): void {}

  async onSubmit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.submitting = true; this.error = undefined;
    const value = this.form.value;
    try {
      await this.todoService.createTask({ title: value.title, description: value.description }).toPromise();
      this.form.reset();
      this.created.emit();
    } catch (err: any) {
      this.error = err?.message ?? 'Failed to create task.';
      console.error(err);
    } finally { this.submitting = false; }
  }
}
