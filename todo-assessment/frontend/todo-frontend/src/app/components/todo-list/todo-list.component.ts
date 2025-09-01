import { Component, OnInit } from '@angular/core';
import { TodoService } from '../../services/todo.service';
import { TaskItem } from '../../models/task_item';
import { TodoFormComponent } from '../todo-form/todo-form.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, TodoFormComponent],
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {
  tasks: TaskItem[] = [];
  loading = false;
  error?: string;

  constructor(private todoService: TodoService) {}

  ngOnInit(): void { this.load(); }

  load() {
    this.loading = true; this.error = undefined;
    this.todoService.getTasks(5).subscribe({
      next: res => { this.tasks = res; this.loading = false; },
      error: err => { console.error(err); this.error = 'Failed to load tasks'; this.loading = false; }
    });
  }

  onCreated() { this.load(); }

  markDone(task: TaskItem) {
    const prev = [...this.tasks];
    this.tasks = this.tasks.filter(t => t.id !== task.id);
    this.todoService.completeTask(task.id).subscribe({
      error: err => { console.error(err); this.tasks = prev; this.error = 'Could not mark task as done.'; }
    });
  }
}
