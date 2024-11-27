import Module from "node:module";

const require = Module.createRequire(import.meta.url);

const prompt = require('prompt-sync')({ sigint: true });


export default class taskInput {


  constructor(taskText) {

    this.taskText = taskText

    this.answer = prompt(taskText)

  }


}