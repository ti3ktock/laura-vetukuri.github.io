import numpy as np  # import the NumPy library as "np" for numerical operations

class SudokuBoard:
    def __init__(self):  # constructor method for SudokuBoard class with "self" parameter
        self.board = np.zeros((9, 9), dtype=int)#initializes a 9x9 numpy array ("board") with 0. 0 = empty cells

    #prints the sudoku board so that it is easier to see and understand. An illustration of the sudoku board
    def print_board(self):
        for i in range(9): #loop that iterates through the rows of the sudoku board
            if i % 3 == 0 and i != 0:  # checks if it is a multiple of 3 and not equal to 0
                print("-" * 21)  # prints 21 "-" every 3 rows to make 3x3 blocks
            for j in range(9):
                if j % 3 == 0 and j != 0:  # checks if it is a multiple of 3 and not equal to 0
                    print("|", end=" ")  # prints "|" every 3 column to make 3x3 blocks

                num = self.board[i][j]  # is used to get the values of i and j s position
                if num == 0: #checks if it is an empty cell , that is if it is a 0
                    print("*", end=" ")  # prints a "*" for empty cells (change from 0 to *)
                else:
                    print(num, end=" ") # prints the number for non-empty cells
            print()  # new line after every row

    #this method checks if the move is valid or not
    def check_valid(self, row, col, num):
        for x in range(9):  #checks the entire row and column for num
            if self.board[row, x] == num or self.board[x, col] == num:  #checks if num already exists in the row or column else it will return false- not vailid to place num (9x9 block)
                return False
            startRow = row - row % 3 # starting row of 3x3 block
            startCol = col - col % 3 # starting column of 3x3 block
            for i in range(3):
                for j in range(3):
                    if self.board[i+ startRow, j + startCol] == num:
                        return False # check if num already exists in the 3x3 block, return false(not valid to place num) if it is found
            return True #if num is not found in row , column or3x3, it returns True - it is ok to place the number

    # a method that solves the sudoku
    def solve_sudoku(self):
        for row in range(9):
            for col in range(9):
                if self.board[row][col] == 0: #check if the value is 0
                    for num in range(1,10): # try numbers from 1-9
                        if self.check_valid(row, col, num):
                            self.board[row][col] = num
                            if self.solve_sudoku():
                                return True
                            self.board[row][col] = 0
                    return False
        return True
if __name__ == "__main__":
    sudoku = SudokuBoard() # creates an instance of the SudokuBoard class
    intial_board = [[5,3,0,0,7,0,0,0,0],        #example sudoku as a list of lists
                    [6,0,0,1,9,5,0,0,0],
                    [0,9,8,0,0,0,0,6,0],
                    [8,0,0,0,6,0,0,0,3],
                    [4,0,0,8,0,3,0,0,1],
                    [7,0,0,0,2,0,0,0,6],
                    [0,6,0,0,0,0,2,8,0],
                    [0,0,0,4,1,9,0,0,5],
                    [0,0,0,0,8,0,0,7,9]
                    ]
    sudoku.board = np.array(intial_board)  #sets the initial Sudoku

    print("Sudoku: ")
    sudoku.print_board()  # prints the sudoku board

    if sudoku.solve_sudoku():
        print("\n Solved sudoku: ")
        sudoku.print_board() # tries to solve the sudoku and if it is solved it will print the solution
    else:
        print("\n no solution")# if there is no solution to the sudoku it prints the string "no solution"


