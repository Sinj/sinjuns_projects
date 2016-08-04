//Matrix.h file
#ifndef __Matrix_H_INCLUDED__   // if x.h hasn't been included yet...
#define __Matrix_H_INCLUDED__   //   #define this so the compiler knows it has been included

class Matrix{

public:
	//if you didnt have a data set
	Matrix(int sizeR, int sizeC, double val);
	//if you have data to work with
	Matrix(int sizeR, int sizeC, double* pData);
	//Copy constructor
	Matrix(const Matrix& X);
	//call the destructor class
	~Matrix();

	//Operators
	Matrix operator+(const Matrix& X);
	Matrix operator-(const Matrix& X);
	Matrix operator*(const Matrix& X);
	Matrix operator/(const Matrix& X);
	Matrix operator=(const Matrix& X);
	Matrix operator++();//increment
	Matrix operator--();//decrement
	double operator()(int i, int j)const;  // Operator () == get function

	//get the value from the matrix class data
	double get(int i, int j) const;// const will stop the function from changing values in the matrix

	//set a new value for a element in the martix class data
	void set(int i, int j, double val);

	//Matrix getblock 
	Matrix getblock(int start_row, int end_row, int start_col, int end_col);

	//get M
	int getM()const;

	//get N
	int getN()const;

	//output the whole matrix
	void Output_Matrix(int row, int col);

protected:
	int M; //number of rows
	int N; //number of columns
	double* data;

};
#endif 