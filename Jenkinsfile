pipeline {
    agent any

    tools {
        dotnetsdk 'dotnet8'
    }

    stages {

        stage('Restore') {
            steps {
                sh 'dotnet restore UrlHealthMonitor.sln'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build UrlHealthMonitor.sln --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test UrlHealthMonitor.sln --no-build --logger "trx;LogFileName=test-results.trx"'
            }
            post {
                always {
                    junit '**/*.trx'
                }
            }
        }
    }

    post {
        success {
            echo 'Build and Tests Successful ✅'
        }
        failure {
            echo 'Build Failed ❌'
        }
    }
}
