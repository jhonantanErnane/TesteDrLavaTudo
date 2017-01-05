angular.module("app",[])
.controller('relatorioCtrl', ['$scope','$http', function($scope,$http){
	$scope.busca = true;
	$scope.ordemServico = true;
	$scope.clienteOS = true;
	$scope.titulo = "Dr Lava Tudo Relatórios";

	$scope.relatorios = [{
		value: 'os',
		label: 'Procurar por uma ordem de serviço'
	}, {
		value: 'cliente',
		label: "Mostrar todas as OS's de um cliente"
	}];

	$scope.mudar = function(){
		if ($scope.listaRelatorios.value == 'cliente' ) 
		{
			$scope.placeholder = 'Digite o codigo da ordem de serviço.';
			$scope.busca = false;
		}
		else
		{
			$scope.placeholder = 'Digite o codigo do cliente que deseja procurar.';
			$scope.busca = false;
		}
	}

	$scope.procurar = function(){
		if (($scope.listaRelatorios.value == 'cliente') && ($scope.criterioBusca))
		{
			$scope.procurarCliente();
		}
		else if (($scope.listaRelatorios.value == 'os') && ($scope.criterioBusca))
		{
			$scope.procurarOs();
		}
	};
	$scope.procurarOs = function(){
		$http({
			method: 'GET',
			url: 'http://localhost:64392/ui/os/' + $scope.criterioBusca
		}).then(function successCallback(response) {
			$scope.os = [];
			$scope.itensOs = [];
			for (var i = 0 ; i < response.data.length  ; i++) {
				$scope.os.push(response.data[i]);
			}
			for (var i = 0; i < $scope.os[2].length; i++) {
				$scope.itensOs.push($scope.os[2][i]);
			}
			for (var i = 0; i < $scope.itensOs.length; i++) {
				$scope.itensOs[i].valorfinal = parseFloat($scope.itensOs[i].valorfinal.toFixed(2));
			}

			if ($scope.clienteOS)
				$scope.clienteOS = false;

			$scope.ordemServico = true;
			
		}, function errorCallback(response) {
			console.log('erro');
		});
	};

	$scope.procurarCliente = function(){
		$http({
			method: 'GET',
			url: 'http://localhost:64392/ui/cliente/' + $scope.criterioBusca
		}).then(function successCallback(response) {
			$scope.cliente = [];
			for (var i = 0 ; i < response.data[0].length  ; i++) {
				$scope.cliente.push(response.data[0][i]);
			}
			
			if ($scope.ordemServico)
				$scope.ordemServico = false;

			$scope.clienteOS = true;

		}, function errorCallback(response) {
			console.log('erro');
		});
	};
	

}]);