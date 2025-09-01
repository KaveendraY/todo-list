export interface TaskItem {
  id: string;
  title: string;
  description?: string | null;
  completed: boolean;
  createdAt: string;
}
