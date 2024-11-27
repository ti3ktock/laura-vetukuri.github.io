

export default class task {
  tasks = [];
  doneTasks = [];
  taskIdCounter = 1; // Initialize a counter for task IDs

  get length() {
    return this.tasks.length;
  }

  get asString() {
    return this.tasks.join("");
  }

  addTask(taskContent) {
    const task = {
      id: this.taskIdCounter++, // Assign a unique ID and increment the counter
      content: taskContent
    };

    this.tasks.push(task);
  }

  //prints all the tasks in the to-do list
  printTasks() {
    for (let task of this.tasks) {
      console.log(`Task ID: ${task.id}, Content: ${task.content}`);
    }
  }

  printDoneTasks() {
    console.log("Done tasks:");
    for (let task of this.doneTasks) {
      console.log(`Task ID: ${task.id}, Content: ${task.content}`);
    }
  }


  removeFromBottom() {
    this.tasks.pop();

  }

  removeFromTop() {
    this.tasks.shift();

  }

  addToTop(taskContent) {
    const task = {
      id: this.taskIdCounter++,
      content: taskContent
    };

    this.tasks.unshift(task);
  }

  removeByIndex(taskIndex) {
    if (taskIndex >= 1 && taskIndex <= this.tasks.length) {
      const removedTask = this.tasks.splice(taskIndex - 1, 1)[0]; 
      console.log(`Removed task with ID ${removedTask.id}`);
    } else {
      // Handle the case where taskIndex is out of bounds
      console.log("Invalid task index. Please enter a valid index.");
    }
    return this.tasks;
  }

  moveToTopByIndex(taskIndex) {
    if (taskIndex >= 1 && taskIndex <= this.tasks.length) {
      const removedTask = this.tasks.splice(taskIndex - 1, 1)[0];
      this.tasks.unshift(removedTask);
    } else {
      console.log("Invalid task index. Please enter a valid index.");
    }
  
  }

  moveToBottomByIndex(taskIndex) {
    if (taskIndex >= 1 && taskIndex <= this.tasks.length) {
      const removedTask = this.tasks.splice(taskIndex - 1, 1)[0];
      this.tasks.push(removedTask); 
    } else {
      console.log("Invalid task index. Please enter a valid index.");
    }
  }

  removeByContent(taskContent) {
    const indexToRemove = this.tasks.findIndex(task => task.content === taskContent);

    if (indexToRemove !== -1) {
      const removedTask = this.tasks.splice(indexToRemove, 1)[0];
      console.log(`Removed task with ID ${removedTask.id} and content "${removedTask.content}"`);
    } else {
      console.log(`Task with content "${taskContent}" not found.`);
    }

    return this.tasks;
  }



  moveDownByIndex(taskIndex) {
    if (taskIndex >= 1 && taskIndex < this.tasks.length) {
      const currentTask = this.tasks[taskIndex - 1];
      this.tasks[taskIndex - 1] = this.tasks[taskIndex];
      this.tasks[taskIndex] = currentTask;
    } else {
      console.log("Invalid task index. Please enter a valid index to move the task down.");
    }
  }

  moveUpByIndex(taskIndex) {
    if (taskIndex > 1 && taskIndex <= this.tasks.length) {
      const currentTask = this.tasks[taskIndex - 1];
      this.tasks[taskIndex - 1] = this.tasks[taskIndex - 2];
      this.tasks[taskIndex - 2] = currentTask;
    } else {
      console.log("Invalid task index. Please enter a valid index to move the task up.");
    }
  }

  moveToDoneByIndex(taskIndex) {
    if (taskIndex >= 1 && taskIndex <= this.tasks.length) {
      const movedTask = this.tasks.splice(taskIndex - 1, 1)[0];
      this.doneTasks.push(movedTask);
      console.log(`Moved task with ID ${movedTask.id} to the "done" list.`);
    } else {
      console.log("Invalid task index. Please enter a valid index to move the task to the 'done' list.");
    }
  }
  // Save tasks to a file
  saveTasksToFile() {
    const tasksData = JSON.stringify(this.tasks, null, 2);
    writeFileSync('tasks.json', tasksData, 'utf8');
    console.log('Tasks saved to tasks.json');
  }

  // Load tasks from a file
  loadTasksFromFile() {
    try {
      const tasksData = readFileSync('tasks.json', 'utf8');
      this.tasks = JSON.parse(tasksData);
      console.log('Tasks loaded from tasks.json');
    } catch (error) {
      console.log('Error loading tasks from tasks.json');
    }
  }

 



  constructor(list) {
    this.tasks = this.processWord(list);
  }

  processWord(list) {
    return list.split("");
  }
}
