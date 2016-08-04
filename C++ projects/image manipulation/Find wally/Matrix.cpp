#include <iostream>
#include "Matrix.h"

using namespace std;

//Matrix constructors
Matrix::Matrix(int sizeR, int sizeC, double val){
	//cout << "Matrix(int, int, double) Constructor is invoked ..." << endl;
	M = sizeR;
	N = sizeC;
	data = new double[M*N];

	for (int ii = 0; ii < M; ii++){
		for (int jj = 0; jj < N; jj++){
			data[ii*N + jj] = val;
		}
	}
}
Matrix::Matrix(int sizeR, int sizeC, double* pData){
	//cout << "Matrix(int, int, double*) Constructor is invoked ..." << endl;
	M = sizeR;
	N = sizeC;

	data = new double[M*N];

	for (int ii = 0; ii < M; ii++){
		for (int jj = 0; jj < N; jj++){
			data[ii*N + jj] = pData[ii*N + jj];
		}
	}

}
Matrix::Matrix(const Matrix& X)
{
	//cout << "Matrix::Matrix(const Matrix& X) is invoked" << endl;

	M = X.M;
	N = X.N;
	data = new double[M*N];

	for (int i = 0; i < M*N; i++)
	{
		data[i] = X.data[i];
	}
}
//deconstructor
Matrix::~Matrix()
{
	/*cout <<"~Matrix() Destructor is invoked ..." << endl;*/
	delete[] data;
}
//Matrix operators 
Matrix Matrix::operator+(const Matrix& X)
{
	//cout << "Matrix::operator+ is invoked..." << endl;
	double val = 0;
	Matrix Z(M, N, val);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] + X(i, j);
		Z.set(i, j, val);
	}
	return Z;
}
Matrix Matrix::operator-(const Matrix& X)
{//A-B
	//cout << "Matrix::operator- is invoked..." << endl;
	double val = 0;
	Matrix Z(M, N, val);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] - X(i, j);
		Z.set(i, j, val);
	}
	return Z;
}
Matrix Matrix::operator*(const Matrix& X)
{
	//cout << "Matrix::operator* is invoked..." << endl;
	double val = 0;
	Matrix Z(M, N, val);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		val = data[i*N + j] * X(i, j);
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
		val = data[i*N + j] / X(i, j);
		Z.set(i, j, val);
	}
	return Z;
}
Matrix Matrix::operator=(const Matrix& X)
{
	//cout << "Matrix::operator= is invoked..." << endl;
	//for (int i = 0; i < 31; i++)    //This loops on the rows.
	//{
	//	for (int j = 0; j < 31; j++) //This loops on the columns
	//	{
	//		cout << X.data[i*N + j] << " ";
	//	}
	//	cout << endl;
	//}



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

		//make new data
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
	//cout << "Matrix::operator++ is invoked..." << endl;
	for (int i = 0; i < M; i++){
		for (int j = 0; j < N; j++){
			set(i, j, get(i, j) + 1);
		}
	}
	return *this;
}
Matrix Matrix::operator-- ()
{
	//cout << "Matrix::operator-- is invoked..." << endl;
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
	cout << "Matrix::operator() is invoked..." << endl;
	return data[i*N + j];
}

//get, get a value from data
double Matrix::get(int i, int j) const
{
	return data[i*N + j];
}

//set, set a a vale to data
void Matrix::set(int i, int j, double val)
{
	data[i*N + j] = val;
}

//GetM, get the value M from class data member
int Matrix::getM()const
{
	return M;
}
//GetN, get the value N from class data member
int Matrix::getN()const
{
	return N;
}

//Out_Matrix, output from data, using Row and col to define where
void Matrix::Output_Matrix(int row, int col){
	for (int i = 0; i < row; i++)    //This loops on the rows.
	{
		for (int j = 0; j < col; j++) //This loops on the columns
		{
			cout << data[i*N + j] << "";
		}
		cout << endl;
	}
};

//Matrix get block 
Matrix Matrix::getblock(int start_row, int end_row, int start_col, int end_col)
{
	//cout << "getblock(int start_row, int end_row, int start_col, int end_col) is invoked" << endl;


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