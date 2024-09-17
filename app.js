var app = angular.module('todoApp', []);

app.constant('ApiConfig', {
    baseUrl: 'https://localhost:7089'
});

app.controller('MainController', function ($scope, $http, ApiConfig) {
    $scope.isLoggedIn = false;
    $scope.todos = [];
    $scope.currentPage = 0;
    $scope.pageSize = 2;
    $scope.statusFilter = null;
    $scope.isModalOpen = false;
    $scope.isEditing = false;
    $scope.modalData = {};
    $scope.loginData = {};
    $scope.hasMoreItems = false;

    $scope.login = function () {
        $http({
            method: 'POST',
            url: ApiConfig.baseUrl + '/Users/Login',
            data: JSON.stringify($scope.loginData),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            if (response.data.success) {
                $scope.isLoggedIn = true;
                $scope.getTodos();
            } else {
                alert('Login falhou!');
            }
        });
    };

    // Buscar To-dos
    $scope.getTodos = function () {
        const params = {
            status: $scope.statusFilter,
            pageIndex: $scope.currentPage,
            pageSize: $scope.pageSize
        };
        $http({
            method: 'GET',
            url: ApiConfig.baseUrl + '/Todos',
            params: params,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            if (response.data.data == null) {
                $scope.todos = [];
            } else {
                $scope.todos = response.data.data;
            }

            $scope.hasMoreItems = $scope.todos?.length == $scope.pageSize;
        });
    };

    $scope.openModal = function (todo) {
        $scope.isEditing = !!todo;
        $scope.modalData = todo ? angular.copy(todo) : {};
        $scope.isModalOpen = true;

        $('.modal').modal('show');
    };

    $scope.closeModal = function () {
        $scope.isModalOpen = false;
        $scope.modalData = {};

        $('.modal').modal('hide');
    };

    $scope.saveTodo = function () {
        if ($scope.isEditing) {
            $http({
                method: 'PUT',
                url: ApiConfig.baseUrl + '/Todos',
                data: JSON.stringify($scope.modalData),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function () {
                $scope.closeModal();
                $scope.getTodos();
            });
        } else {
            $http({
                method: 'POST',
                url: ApiConfig.baseUrl + '/Todos',
                data: JSON.stringify($scope.modalData),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function () {
                $scope.closeModal();
                $scope.getTodos();
            });
        }
    };

    $scope.updateStatus = function (todo) {
        $http({
            method: 'PATCH',
            url: ApiConfig.baseUrl + '/Todos/UpdateStatus/' + todo.id,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function () {
            $scope.getTodos();
        });
    };

    $scope.deleteTodo = function (id) {
        var confirmed = window.confirm('Realmente deseja excluir este item?');

        if (confirmed) {
            $http({
                method: 'DELETE',
                url: ApiConfig.baseUrl + '/Todos/' + id,
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function () {
                $scope.getTodos();
            });
        }
    };

    $scope.nextPage = function () {
        $scope.currentPage++;
        $scope.getTodos();
    };

    $scope.previousPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.getTodos();
        }
    };

    $scope.updateStatusFilter = function (status) {
        $scope.statusFilter = status;
        $scope.getTodos();
    };
});
