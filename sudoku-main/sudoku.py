import numpy as np  # import the NumPy library as "np" for numerical operations

import random #imported the random module to be able to randomly choose a sudoku puzzle
from sudoku_puzzles import puzzles #imports the object puzzle from sudoku_puzzles python file

class SudokuBoard:
    def __init__(self):  # constructor method for SudokuBoard class with "self" parameter
        self.board = np.zeros((9, 9), dtype=int)  # initializes a 9x9 numpy array ("board") with 0. 0 = empty cells

    # prints the sudoku board so that it is easier to see and understand. An illustration of the sudoku board
    def print_board(self):
        print("    A B C   D E F   G H I")  # letters above the sudoku columns
        for i in range(9):  # loop that iterates through the rows of the sudoku board
            if i % 3 == 0 and i != 0:  # checks if it is a multiple of 3 and not equal to 0
                print("-" * 25)  # prints 25 "-" every 3 rows to make 3x3 blocks
            print(f"{i + 1} |",
                  end=" ")  # print number tills there is a row incrementally  and add "|" after  and a space to the end
            for j in range(9):
                if j % 3 == 0 and j != 0:  # checks if it is a multiple of 3 and not equal to 0
                    print("|", end=" ")  # prints "|" every 3 column to make 3x3 blocks

                num = self.board[i][j]  # is used to get the values of i and j s position
                if num == 0:  # checks if it is an empty cell , that is if it is a 0
                    print("*", end=" ")  # prints a "*" for empty cells (change from 0 to *)
                else:
                    print(num, end=" ")  # prints the number for non-empty cells
            print()  # new line after every row

    # this method checks if the move is valid or not
    def check_valid(self, row, col, num):
        for x in range(9):  # a loop that checks/itterates over the entire row and column for num
            if self.board[row, x] == num or self.board[
                x, col] == num:  # checks if num already exists in the row or column else it will return false- not vailid to place num (9x9 block)
                return False
            startRow = row - row % 3  # starting row of 3x3 block
            startCol = col - col % 3  # starting column of 3x3 block
            for i in range(3):#innitiates a loop that iterates over the range from 0 to 2 (i=0,1,2)
                for j in range(3):# same as above
                    if self.board[i + startRow, j + startCol] == num:# checks if the condition is trues
                        return False  # check if num already exists in the 3x3 block, return false(not valid to place num) if it is found
        return True  # if num is not found in row , column or3x3, it returns True - it is ok to place the number

    # placing the number in the array list/in the sudoku
    def place_num(self, position, num):
        if len(position) == 2 and position[0].isalpha() and position[1].isdigit():# checks if the position argument has the length of 2 characters. if the first in dex is an alphabet abd if the second character is a digit, makes sure that it is a correct argument
            col = ord(position[0].upper()) - ord('A')#converst the position to the right column. (upper() converts the character into capital letter)
            row = int(position[1]) - 1#gets the numeric part of the index and takes away one number to convert it to its right index

            if 0 <= row < 9 and 0 <= col < 9 and 1 <= num <= 9:# checks if the number is between 1 and 9 (0-8 index number)
                if self.check_valid(row, col, num): # checks if the number is all right to use according to the rules (check_valid)
                    self.board[row][col] = num#if everything is right, the cell will be assigned with the inputed number
                    return True # the number is placed now
        return False # if the above conditions are not met, than it will not add it to the sudoku board

    # checks if the sudoku is solved according to the solved_board list
    def is_solved(self, solution_key):
        return np.array_equal(self.board, solution_key)# compares the sudoku board(self.board) with the solution_key from the sudoku_puzzles. The comparision is made by a NumPy function

# # a method that solves the sudoku
    def solve_sudoku(self):
        for row in range(9): #it loops through the rows
            for col in range(9):#loops through the columns
                if self.board[row][col] == 0: #check if the value is 0
                    for num in range(1,10): # try numbers from 1-9
                        if self.check_valid(row, col, num): # calls for the check_valid method to check if it can add num to the 3x3 block; if, yes it proceeds to the next check
                            self.board[row][col] = num # this is for in case the number is not valid in the following "if" statement it will come back to this step and sets it to 0; and it will go to next step again with another number
                            if self.solve_sudoku(): # calls for the solve_sudoku method, constantly checking if it can fill the next cell
                                return True
                            self.board[row][col] = 0
                    return False # if the board is not solvable.
        return True # if the sudoku is solved it is true

if __name__ == "__main__":
    sudoku = SudokuBoard()  # creates an instance of the SudokuBoard class, initializes with empty cells

    puzzle_keys = list(puzzles.keys())#gets a list of keys from the puzzle dictionary
    random_key=random.choice(puzzle_keys)#chooses a random key from the above
    selected_puzzle = puzzles[random_key]# selects the puzle according to the random_key

    sudoku.board = np.array(selected_puzzle)  # the selected_puzzle becomes the intial board with the help of NumPy
    print("Sudoku: ")

    while True: # while the conditions are met the program will run
        sudoku.print_board() # prints the sudoku board
        position = input("Enter position (ex: A1,A2,C6...): ").strip() #the user can enter an input after which the code will strip the string
        num = int(input("Enter a number (1-9) for the cell: "))# the user can enter a number, the string is turned into an integer.

        if sudoku.place_num(position, num): #a condtitional statment to see if the statement is right or not. in this case it checks if it is a valid position and number
            if sudoku.is_solved(random_key):# a conditional statment where the board and a statemnt is printed if the the board is the same as the random_key.
                print("\nSolved Sudoku: ") # prints out a statemnt
                sudoku.print_board()#prints out the board
                print("\nYou rock!!")#prints out a statemnt
                break  # End the loop as the Sudoku is solved
            else: # if the conditional statemnt is not met, i.e. the sudoku board is not the same as the solved_board array list, but it is the right number it prints the following text
                print("YAY! You got the right number!")
                sudoku.print_board()
        else:# an else statement for the first if statemnt, that prints out a statement if the input was wrong
            print("Neh! Wrong number or already used field!")

        solve_sudoku= input("Would you like to see the solved sudoku? (Y/N): ").strip()#makes it possible to type into the console "y" or "n" if the user wants to see the solved sudoku puzzle
        if solve_sudoku.upper() == "Y": # conditioal statemnt where if the user types the letter "y" (either upper or lowe case) the following will happen...
            if sudoku.solve_sudoku():#if the sudoky is solved according to the solve_sudoku() method than it will...
                print("\n Solved sudoku: ")#print that String...
                sudoku.print_board()  # tries to solve the sudoku and if it is solved it will print the solution
                break #exits the sudoku game
            else:#if the sudoku is not solved
                print("\n no solution")  # if there is no solution to the sudoku it prints the string "no solution"
        exit = input("Would you like to exit the game? (Y/N): ").strip() # the user is asked if the user would like to exits the game, if "y".... (.strip()-removes the whitespace from the string)
        if exit.upper() == "Y":# if the answer is "y".....
            print("Thank you for playing!")# it will print the String and...
            break#exit the game
