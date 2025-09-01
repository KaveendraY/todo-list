import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';
import { Observable } from 'rxjs';
import { TaskItem } from '../models/task_item';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private base = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getTasks(limit = 5): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(`${this.base}/api/tasks?limit=${limit}`);
  }
  createTask(payload: { title: string; description?: string }) {
    return this.http.post<TaskItem>(`${this.base}/api/tasks`, payload);
  }
  completeTask(id: string) {
    return this.http.post(`${this.base}/api/tasks/${id}/complete`, {});
  }
}
