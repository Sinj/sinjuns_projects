//Matrix function .cpp file
#include <iostream>
#include "Matrix.h"

using namespace std;

//Matrix constructors
Matrix::Matrix(int sizeR, int sizeC, double val){
	//Set Row size to M
	//set coloumn size to N
	M = sizeR;
	N = sizeC;
	data = new double[M*N];

	//save the val to elements
	for (int ii = 0; ii < M; ii++){
		for (int jj = 0; jj < N; jj++){
			data[ii*N + jj] = val;
		}
	}
}
Matrix::Matrix(int sizeR, int sizeC, double* pData){
	
	M = sizeR;
	N = sizeC;
	
	data = new double[M*N];
	//copy pdata to data
	for (int ii = 0; ii < M; ii++){
		for (int jj = 0; jj < N; jj++){
			data[ii*N + jj] = pData[ii*N + jj];
		}
	}

}
//copy constructor
Matrix::Matrix(const Matrix& X)
{
	//copy M & N 
	M = X.M;
	N = X.N;
	data = new double[M*N];
	//copy data
	for (int i = 0; i < M*N; i++)
	{
		data[i] = X.data[i];
	}
}
//deconstructor
Matrix::~Matrix()
{
	//delete data
	delete[] data;
}
//Matrix operators 
Matrix Matrix::operator+(const Matrix& X)
{
	
	double val = 0;
	Matrix Z(M, N, val); //make blank matrix
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] + X(i, j); //add both elements togather
		Z.set(i, j, val); // save val to matrix
	}
	return Z;
}
Matrix Matrix::operator-(const Matrix& X)
{
	
	double val = 0;
	Matrix Z(M, N, val);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] - X(i, j);//subtract element from element
		Z.set(i, j, val);
	}
	return Z;
}
Matrix Matrix::operator*(const Matrix& X)
{
	
	double val = 0;
	Matrix Z(M, N, val);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] * X(i, j);//multiply elements togather
		Z.set(i, j, val);
	}
	return Z;
}
Matrix Matrix::operator/(const Matrix& X)
{
	//cout << "Matrix::operator/ is invoked..." << endl;
	double val = 0;
	Matrix Z(M, N, val);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] / X(i, j); // divide element from element
		Z.set(i, j, val);
	}
	return Z;
}
Matrix Matrix::operator=(const Matrix& X)
{

	//check if X == X
	if (&X == this)
	{
		return X;
	}
	else
	{

		//delete current data
		delete[] data;

		//overwrite M and N
		M = X.getM();
		N = X.getN();

		//Set new data
		for (int i = 0; i < M; i++){
			for (int j = 0; j < N; j++){
				set(i, j, X.data[i*N + j]);
			}
		}
		return *this;
	}
}
Matrix Matrix::operator++ ()
{
	//this will add one to each element in this matrix
	for (int i = 0; i < M; i++){
		for (int j = 0; j < N; j++){
			set(i, j, get(i, j) + 1);
		}
	}
	return *this;
}
Matrix Matrix::operator-- ()
{
	//this will reomve one form each element in this matrix
	{
		for (int i = 0; i < M; i++){
			for (int j = 0; j < N; j++){
				set(i, j, get(i, j) - 1);
			}
		}
		return *this;
	}
}
double Matrix::operator()(int i, int j)const
{
	//same as get function, will get a value from data
	return data[i*N + j];
}

//get a value from data
double Matrix::get(int i, int j) const
{
	return data[i*N + j];
}

//set a value to data
void Matrix::set(int i, int j, double val)
{
	data[i*N + j] = val;
}

//get the value M from class data member
int Matrix::getM()const
{
	return M;
}
//get the value N from class data member
int Matrix::getN()const
{
	return N;
}

//output from data, using Row and col to define where
void Matrix::Output_Matrix(int row, int col){
	for (int i = 0; i < row; i++)    //This loops on the rows.
	{
		for (int j = 0; j < col; j++) //This loops on the columns
		{
			cout << data[i*N + j] << "";//out each element with a space after it.
		}
		cout << endl;
	}
};

//Matrix get block 
Matrix Matrix::getblock(int start_row, int end_row, int start_col, int end_col)
{
	//get a block of data from another matrix to make a new matrix
	Matrix X(end_row - start_row + 1, end_col - start_col + 1, 0.0);

	for (int i = start_row; i <= end_row; i++)
	{
		for (int j = start_col; j <= end_col; j++)
		{
			X.set(i - start_row, j - start_col, data[i*N + j]);

		}
	}
	return X;
}