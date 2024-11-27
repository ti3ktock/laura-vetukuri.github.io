import { readFileSync, writeFileSync } from 'fs';

export function loadTasksFromFile(taskList) {
  try {
    const tasksData = readFileSync('./tasks.json', 'utf8');
    taskList.tasks = JSON.parse(tasksData);
    console.log('Tasks loaded from tasks.json');
  } catch (error) {
    console.log('Error loading tasks from tasks.json');
  }
}

export function saveTasksToFile(taskList) {
  const tasksData = JSON.stringify(taskList.tasks, null, 2);
  writeFileSync('./tasks.json', tasksData, 'utf8');
  console.log('Tasks saved to tasks.json');
}
