using System.ComponentModel;
using RuntimeHelpers;
using RuntimeHelpers.Mapping;

namespace SAPRFCLib.Core
{
    [Description("Class for defining and raising SAP ABAP Interface specific exception information.")]
    public abstract class AbapExceptions : GenericEnumeration
    {
        #region ExposedAccessors

        public static readonly AbapExceptions InternalExecutionException = new AbapInternalException();
        public static readonly AbapExceptions ProcessInexistentException = new AbapProcessInexistentException();
        public static readonly AbapExceptions InvalidOperationException = new AbapInternalInvalidOperationException();
        public static readonly AbapExceptions ExecutionException = new AbapExecutionException();
        public static readonly AbapExceptions InvalidConfigurationException = new AbapInvalidConfigurationException();
        public static readonly AbapExceptions PermissionException = new AbapPermissionException();
        public static readonly AbapExceptions HostnameException = new AbapHostnameException();
        public static readonly AbapExceptions LockedUserException = new AbapLockedUserException();
        public static readonly AbapExceptions InconsistentLockException = new AbapInconsistentLockException();
        public static readonly AbapExceptions UnlockUserException = new AbapUnlockUserException();
        public static readonly AbapExceptions InvalidUserComputerException = new AbapInvalidUserComputerException();
        public static readonly AbapExceptions InvalidHostNameException = new AbapInvalidHostNameException();
        public static readonly AbapExceptions AgentCredentialsMissingException = new AbapAgentCredentialsMissingException();
        public static readonly AbapExceptions MissingNetworkConfigurationException = new AbapMissingNetworkConfigurationException();
        public static readonly AbapExceptions InvalidInterfaceException = new AbapInvalidInterfaceException();
        public static readonly AbapExceptions InvalidNetConnectionException = new AbapInvalidNetConnectionException();
        public static readonly AbapExceptions NullOrEmptyNetConnectionException = new AbapNullOrEmptyNetConnectionException();
        public static readonly AbapExceptions ServerReferenceException = new AbapServerReferenceException();
        public static readonly AbapExceptions ServiceReferenceException = new AbapServiceReferenceException();
        public static readonly AbapExceptions IsLockedConnectionException = new AbapIsLockedConnectionException();
        public static readonly AbapExceptions NetworkLockFailedException = new AbapNetworkLockFailedException();
        public static readonly AbapExceptions NetworkUnlockFailedException = new AbapNetworkUnlockFailedException();
        public static readonly AbapExceptions EmptyPoolException = new AbapEmptyPoolException();
        public static readonly AbapExceptions InvalidPoolException = new AbapInvalidPoolException();
        public static readonly AbapExceptions PoolAlreadyLockedException = new AbapPoolAlreadyLockedException();
        public static readonly AbapExceptions PoolLockFailedException = new AbapPoolLockFailedException();
        public static readonly AbapExceptions PoolUnlockFailedException = new AbapPoolUnlockFailedException();
        public static readonly AbapExceptions PoolInvalidServiceException = new AbapPoolInvalidServiceException();
        public static readonly AbapExceptions NullServiceReferenceException = new AbapNullServiceReferenceException();
        public static readonly AbapExceptions NullServiceUnexpectedException = new AbapNullServiceUnexpectedException();
        public static readonly AbapExceptions ServiceLockedException = new AbapServiceLockedException();
        public static readonly AbapExceptions EmptyOperationException = new AbapEmptyOperationException();
        public static readonly AbapExceptions InvalidOperationRequestException = new AbapInvalidOperationRequestException();
        public static readonly AbapExceptions EmptyOperationParameterException = new AbapEmptyOperationParameterException();
        public static readonly AbapExceptions MissingServiceException = new AbapMissingServiceException();
        public static readonly AbapExceptions InvalidResourceException = new AbapInvalidResourceException();
        public static readonly AbapExceptions InstanceAlertException = new AbapInstanceAlertException();
        public static readonly AbapExceptions NullProcessException = new AbapNullProcessException();
        public static readonly AbapExceptions InstanceElementException = new AbapInstanceElementException();
        public static readonly AbapExceptions InsuficientRequirementsException = new AbapInsuficientRequirementsException();
        public static readonly AbapExceptions InvalidAutoPolicyException = new AbapInvalidAutoPolicyException();
        public static readonly AbapExceptions UnspecifiedAutoPolicyException = new AbapUnspecifiedAutoPolicyException();
        public static readonly AbapExceptions EmptyAutoSelectorException = new AbapEmptyAutoSelectorException();
        public static readonly AbapExceptions UnavailableServiceElementException = new AbapUnavailableServiceElementException();
        public static readonly AbapExceptions ImproperServiceDataException = new AbapImproperServiceDataException();
        public static readonly AbapExceptions EmptyNoteException = new AbapEmptyNoteException();
        public static readonly AbapExceptions EmptyTaskIdException = new AbapEmptyTaskIdException();
        public static readonly AbapExceptions InvalidTaskFromIdException = new AbapInvalidTaskFromIdException();
        public static readonly AbapExceptions IllegitIdChangeException = new AbapIllegitIdChangeException();
        public static readonly AbapExceptions InvalidValidationResultException = new AbapInvalidValidationResultException();
        public static readonly AbapExceptions InvalidEntityIdException = new AbapInvalidEntityIdException();
        public static readonly AbapExceptions InvalidTypeException = new AbapInvalidTypeException();
        public static readonly AbapExceptions InvalidStateException = new AbapInvalidStateException();
        public static readonly AbapExceptions InvalidLicenseException = new AbapInvalidLicenseException();
        public static readonly AbapExceptions CustomParameterParsingException = new AbapCustomParameterParsingException();

        #endregion

        #region Private Exception Classes

        public AbapExceptions(int value, string name) : base(value, name)
        {
        }
        
        public abstract string ErrorDescription { get; }
        public abstract int AbapCode { get; }
        public abstract string AbapName { get; }
        
        private sealed class AbapInternalException : AbapExceptions
        {

            public AbapInternalException() : base(1, "AbapInternalException")
            {
                
            }

            public override string ErrorDescription => "Internal exception occurred during configuration";

            public override int AbapCode => 1;

            public override string AbapName => "ERROR_CODE_ACC_INTERNAL_ERROR ";
        }
        
        private sealed class AbapProcessInexistentException : AbapExceptions
        {

            public AbapProcessInexistentException() : base(2, "AbapProcessInexistentException")
            {
                
            }

            public override string ErrorDescription => "No such process exists.";

            public override int AbapCode => 2;

            public override string AbapName => "ERROR_CODE_NO_SUCH_PROCESS  ";
        }
        
        private sealed class AbapInternalInvalidOperationException : AbapExceptions
        {

            public AbapInternalInvalidOperationException() : base(3, "AbaInvalidOperationException")
            {
                
            }

            public override string ErrorDescription => "Operation type provided is not valid.";

            public override int AbapCode => 3;

            public override string AbapName => "ERROR_CODE_INVALID_OPERATION_TYPE   ";
        }
        
        private sealed class AbapExecutionException : AbapExceptions
        {

            public AbapExecutionException() : base(4, "AbapExecutionException")
            {
                
            }

            public override string ErrorDescription => "Error occurs during the execution of the operation.";

            public override int AbapCode => 4;

            public override string AbapName => "ERROR_CODE_OPERATION_EXECUTION_ERROR";
        }
        
        private sealed class AbapInvalidConfigurationException : AbapExceptions
        {

            public AbapInvalidConfigurationException() : base(5, "AbapInvalidConfigurationException")
            {
                
            }

            public override string ErrorDescription => "Configuration data provided is not valid.";

            public override int AbapCode => 5;

            public override string AbapName => "ERROR_CODE_INVALID_CONFIGURATION_DATA ";
        }
        
        private sealed class AbapPermissionException : AbapExceptions
        {

            public AbapPermissionException() : base(6, "AbapPermissionException")
            {
                
            }

            public override string ErrorDescription => "User has no permission to perform specific operation.";

            public override int AbapCode => 6;

            public override string AbapName => "ERROR_CODE_USER_HAS_NO_PERMISSION";
        }
        
        private sealed class AbapHostnameException : AbapExceptions
        {

            public AbapHostnameException() : base(7, "AbapHostnameException")
            {
                
            }

            public override string ErrorDescription => "No computer system exists for the host name provided";

            public override int AbapCode => 7;

            public override string AbapName => "ERROR_CODE_NO_SUCH_COMPUTER_SYSTEM  ";
        }
        
        private sealed class AbapLockedUserException : AbapExceptions
        {

            public AbapLockedUserException() : base(8, "AbapLockedUserException")
            {
                
            }

            public override string ErrorDescription => "Computer system already locked for configuration";

            public override int AbapCode => 8;

            public override string AbapName => "ERROR_CODE_COMPUTER_SYSTEM_ALREADY_LOCKED   ";
        }
        
        private sealed class AbapInconsistentLockException : AbapExceptions
        {

            public AbapInconsistentLockException() : base(9, "AbapInconsistentLockException")
            {
                
            }

            public override string ErrorDescription => "Configuration operation cannot receive a lock on the computer system";

            public override int AbapCode => 9;

            public override string AbapName => "ERROR_CODE_COMPUTER_SYSTEM_LOCK_FAILURE    ";
        }
        
        private sealed class AbapUnlockUserException : AbapExceptions
        {

            public AbapUnlockUserException() : base(10, "AbapUnlockUserException")
            {
                
            }

            public override string ErrorDescription => "Error unlocking the computer system";

            public override int AbapCode => 10;

            public override string AbapName => "ERROR_CODE_COMPUTER_SYSTEM_UNLOCK_FAILURE     ";
        }
        
        private sealed class AbapInvalidUserComputerException : AbapExceptions
        {

            public AbapInvalidUserComputerException() : base(11, "AbapInvalidUserComputerException")
            {
                
            }

            public override string ErrorDescription => "No such computer system host name exists";

            public override int AbapCode => 11;

            public override string AbapName => "ERROR_CODE_NO_SUCH_COMPUTER_SYSTEM_HOSTNAME";
        }
        
        private sealed class AbapInvalidHostNameException : AbapExceptions
        {

            public AbapInvalidHostNameException() : base(12, "AbapInvalidHostNameException")
            {
                
            }

            public override string ErrorDescription => "No computer system host name is provided when required";

            public override int AbapCode => 12;

            public override string AbapName => "ERROR_CODE_EMPTY_OR_NULL_COMPUTER_SYSTEM_HOSTNAME ";
        }
        
        private sealed class AbapAgentCredentialsMissingException : AbapExceptions
        {

            public AbapAgentCredentialsMissingException() : base(13, "AbapAgentCredentialsMissingException")
            {
                
            }

            public override string ErrorDescription => "SAP Host Agent credentials are missing for the computer system specified";

            public override int AbapCode => 13;

            public override string AbapName => "ERROR_CODE_HOST_AGENT_CREDENTIALS_MISSING";
        }
        
        private sealed class AbapMissingNetworkConfigurationException : AbapExceptions
        {

            public AbapMissingNetworkConfigurationException() : base(14, "AbapMissingNetworkConfigurationException")
            {
                
            }

            public override string ErrorDescription => "Network configuration details are missing for the computer system specified";

            public override int AbapCode => 14;

            public override string AbapName => "ERROR_CODE_NETWORK_CONFIGURATION_MISSING ";
        }
        
        private sealed class AbapInvalidInterfaceException : AbapExceptions
        {

            public AbapInvalidInterfaceException() : base(16, "AbapInvalidInterfaceException")
            {
                
            }

            public override string ErrorDescription => "Network interface ID is null or empty for the computer system specified.";

            public override int AbapCode => 16;

            public override string AbapName => "ERROR_CODE_EMPTY_OR_NULL_NETWORK_INTERFACE_ID  ";
        }
        
        private sealed class AbapInvalidNetConnectionException : AbapExceptions
        {

            public AbapInvalidNetConnectionException() : base(17, "AbapInvalidNetConnectionException")
            {
                
            }

            public override string ErrorDescription => "Network ID specified in the input data is not valid.";

            public override int AbapCode => 17;

            public override string AbapName => "ERROR_CODE_NO_SUCH_NETWORK   ";
        }
        
        private sealed class AbapNullOrEmptyNetConnectionException : AbapExceptions
        {

            public AbapNullOrEmptyNetConnectionException() : base(18, "AbapNullOrEmptyNetConnectionException")
            {
                
            }

            public override string ErrorDescription => "Network ID specified in the input data is not valid.";

            public override int AbapCode => 18;

            public override string AbapName => "ERROR_CODE_EMPTY_OR_NULL_NETWORK_ID";
        }
        
        private sealed class AbapServerReferenceException : AbapExceptions
        {

            public AbapServerReferenceException() : base(19, "AbapServerReferenceException")
            {
                
            }

            public override string ErrorDescription => "Error retrieving the referenced server details for the network.";

            public override int AbapCode => 19;

            public override string AbapName => "ERROR_CODE_REFERENCED_SERVERS ";
        }
        
        private sealed class AbapServiceReferenceException : AbapExceptions
        {

            public AbapServiceReferenceException() : base(20, "AbapServiceReferenceException")
            {
                
            }

            public override string ErrorDescription => "Error retrieving the referenced instance details for the network.";

            public override int AbapCode => 20;

            public override string AbapName => "ERROR_CODE_REFERENCED_SERVICES  ";
        }
        
        private sealed class AbapIsLockedConnectionException : AbapExceptions
        {

            public AbapIsLockedConnectionException() : base(20, "AbapIsLockedConnectionException")
            {
                
            }

            public override string ErrorDescription => "Network is locked by another user or application.";

            public override int AbapCode => 21;

            public override string AbapName => "ERROR_NETWORK_ALREADY_LOCKED   ";
        }
        
        private sealed class AbapNetworkLockFailedException : AbapExceptions
        {

            public AbapNetworkLockFailedException() : base(22, "AbapNetworkLockFailedException")
            {
                
            }

            public override string ErrorDescription => "Failure obtaining the lock for the network.";

            public override int AbapCode => 22;

            public override string AbapName => "ERROR_NETWORK_LOCK_FAILURE    ";
        }
        
        private sealed class AbapNetworkUnlockFailedException : AbapExceptions
        {

            public AbapNetworkUnlockFailedException() : base(23, "AbapNetworkLockFailedException")
            {
                
            }

            public override string ErrorDescription => "Failure trying the unlock for the network.";

            public override int AbapCode => 23;

            public override string AbapName => "ERROR_NETWORK_UNLOCK_FAILURE";
        }
        
        private sealed class AbapEmptyPoolException : AbapExceptions
        {

            public AbapEmptyPoolException() : base(24, "AbapEmptyPoolException")
            {
                
            }

            public override string ErrorDescription => "Pool ID specified in the input data of a method is empty or null.";

            public override int AbapCode => 24;

            public override string AbapName => "ERROR_CODE_EMPTY_OR_NULL_POOL_ID ";
        }
        
        private sealed class AbapInvalidPoolException : AbapExceptions
        {

            public AbapInvalidPoolException() : base(25, "AbapInvalidPoolException")
            {
                
            }

            public override string ErrorDescription => "Pool ID specified in the input data is not valid.";

            public override int AbapCode => 25;

            public override string AbapName => "ERROR_CODE_NO_SUCH_POOL  ";
        }
        
        private sealed class AbapPoolAlreadyLockedException : AbapExceptions
        {

            public AbapPoolAlreadyLockedException() : base(26, "AbapPoolAlreadyLockedException")
            {
                
            }

            public override string ErrorDescription => "Pool ID specified in the input data is not valid.";

            public override int AbapCode => 26;

            public override string AbapName => "ERROR_POOL_ALREADY_LOCKED";
        }
        
        private sealed class AbapPoolLockFailedException : AbapExceptions
        {

            public AbapPoolLockFailedException() : base(27, "AbapPoolLockFailedException")
            {
                
            }

            public override string ErrorDescription => "Failure obtaining the lock for the pool.";

            public override int AbapCode => 27;

            public override string AbapName => "ERROR_POOL_LOCK_FAILURE ";
        }
        
        private sealed class AbapPoolUnlockFailedException : AbapExceptions
        {

            public AbapPoolUnlockFailedException() : base(28, "AbapPoolUnlockFailedException")
            {
                
            }

            public override string ErrorDescription => "Failure trying to unlock the pool.";

            public override int AbapCode => 28;

            public override string AbapName => "ERROR_POOL_UNLOCK_FAILURE  ";
        }
        
        private sealed class AbapPoolInvalidServiceException : AbapExceptions
        {

            public AbapPoolInvalidServiceException() : base(29, "AbapPoolInvalidServiceException")
            {
                
            }

            public override string ErrorDescription => "Service ID provided is not valid.";

            public override int AbapCode => 29;

            public override string AbapName => "ERROR_CODE_INVALID_SERVICE_ID   ";
        }
        
        private sealed class AbapNullServiceReferenceException : AbapExceptions
        {

            public AbapNullServiceReferenceException() : base(30, "AbapNullServiceReferenceException")
            {
                
            }

            public override string ErrorDescription => "No instance matching the input service ID.";

            public override int AbapCode => 30;

            public override string AbapName => "ERROR_CODE_NO_SUCH_SERVICE    ";
        }
        
        private sealed class AbapNullServiceUnexpectedException : AbapExceptions
        {

            public AbapNullServiceUnexpectedException() : base(31, "AbapNullServiceUnexpectedException")
            {
                
            }

            public override string ErrorDescription => "No service ID provided where expected.";

            public override int AbapCode => 31;

            public override string AbapName => "ERROR_CODE_EMPTY_OR_NULL_SERVICE_ID     ";
        }
        
        private sealed class AbapServiceLockedException : AbapExceptions
        {

            public AbapServiceLockedException() : base(32, "AbapServiceLockedException")
            {
                
            }

            public override string ErrorDescription => "Instance already locked for configuration.";

            public override int AbapCode => 32;

            public override string AbapName => "ERROR_CODE_SERVICE_LOCKED      ";
        }
        
        private sealed class AbapEmptyOperationException : AbapExceptions
        {

            public AbapEmptyOperationException() : base(33, "AbapEmptyOperationException")
            {
                
            }

            public override string ErrorDescription => "Operation request is empty.";

            public override int AbapCode => 33;

            public override string AbapName => "ERROR_CODE_EMPTY_OPERATION_REQUEST";
        }
        
        private sealed class AbapInvalidOperationRequestException : AbapExceptions
        {

            public AbapInvalidOperationRequestException() : base(34, "AbapInvalidOperationRequestException")
            {
                
            }

            public override string ErrorDescription => "Operation request is not valid.";

            public override int AbapCode => 34;

            public override string AbapName => "ERROR_CODE_INVALID_OPERATION_REQUEST";
        }
        
        private sealed class AbapEmptyOperationParameterException : AbapExceptions
        {

            public AbapEmptyOperationParameterException() : base(35, "AbapEmptyOperationParameterException")
            {
                
            }

            public override string ErrorDescription => "Operation request item is empty.";

            public override int AbapCode => 35;

            public override string AbapName => "ERROR_CODE_EMPTY_OPERATION_REQUEST_ITEM ";
        }
        
        private sealed class AbapMissingServiceException : AbapExceptions
        {

            public AbapMissingServiceException() : base(36, "AbapMissingServiceException")
            {
                
            }

            public override string ErrorDescription => "Dependent instances are missing for a system.";

            public override int AbapCode => 36;

            public override string AbapName => "ERROR_CODE_DEPEDENT_SERVICES_MISSING  ";
        }
        
        private sealed class AbapInvalidResourceException : AbapExceptions
        {

            public AbapInvalidResourceException() : base(37, "AbapInvalidResourceException")
            {
                
            }

            public override string ErrorDescription => "Selected instance or host is not valid.";

            public override int AbapCode => 37;

            public override string AbapName => "ERROR_CODE_SELECTED_SERVICE_OR_RESOURCE_INVALID   ";
        }
        
        private sealed class AbapInstanceAlertException : AbapExceptions
        {

            public AbapInstanceAlertException() : base(38, "AbapInstanceAlertException")
            {
                
            }

            public override string ErrorDescription => "Instance has an alert.";

            public override int AbapCode => 38;

            public override string AbapName => "ERROR_CODE_SERVICE_HAS_ALERT    ";
        }
        
        private sealed class AbapNullProcessException : AbapExceptions
        {

            public AbapNullProcessException() : base(39, "AbapNullProcessException")
            {
                
            }

            public override string ErrorDescription => "No process ID specified.";

            public override int AbapCode => 39;

            public override string AbapName => "ERROR_CODE_NULL_OR_EMPTY_PROCESS_ID     ";
        }
        
        private sealed class AbapInstanceElementException : AbapExceptions
        {

            public AbapInstanceElementException() : base(40, "AbapInstanceElementException")
            {
                
            }

            public override string ErrorDescription => "No instance element for the value provided.";

            public override int AbapCode => 40;

            public override string AbapName => "ERROR_CODE_NOT_SERVICE_ELEMENT      ";
        }
        
        private sealed class AbapInsuficientRequirementsException : AbapExceptions
        {

            public AbapInsuficientRequirementsException() : base(41, "AbapInsuficientRequirementsException")
            {
                
            }

            public override string ErrorDescription => "Instance does not meet the requirements for an operation.";

            public override int AbapCode => 41;

            public override string AbapName => "ERROR_CODE_UNFULFILL_REQ       ";
        }
        
        private sealed class AbapInvalidAutoPolicyException : AbapExceptions
        {

            public AbapInvalidAutoPolicyException() : base(42, "AbapInvalidAutoPolicyException")
            {
                
            }

            public override string ErrorDescription => "Auto-selection policy specified is not valid.";

            public override int AbapCode => 42;

            public override string AbapName => "ERROR_CODE_INVALID_AUTO_SELECTION_POLICY        ";
        }
        
        private sealed class AbapUnspecifiedAutoPolicyException : AbapExceptions
        {

            public AbapUnspecifiedAutoPolicyException() : base(43, "AbapUnspecifiedAutoPolicyException")
            {
                
            }

            public override string ErrorDescription => "Auto-selection policy is not specified.";

            public override int AbapCode => 43;

            public override string AbapName => "ERROR_CODE_NULL_OR_EMPTY_AUTO_SELECTION_POLICY         ";
        }
        
        private sealed class AbapEmptyAutoSelectorException : AbapExceptions
        {

            public AbapEmptyAutoSelectorException() : base(44, "AbapEmptyAutoSelectorException")
            {
                
            }

            public override string ErrorDescription => "Auto-selector request is empty.";

            public override int AbapCode => 44;

            public override string AbapName => "ERROR_CODE_EMPTY_AUTO_SELECTOR_REQUEST          ";
        }
        
        private sealed class AbapUnavailableServiceElementException : AbapExceptions
        {

            public AbapUnavailableServiceElementException() : base(45, "AbapUnavailableServiceElementException")
            {
                
            }

            public override string ErrorDescription => "Service element is not adaptively installed.";

            public override int AbapCode => 45;

            public override string AbapName => "ERROR_CODE_NOT_ADAPTIVE_SERVICE_ELEMENT           ";
        }
        
        private sealed class AbapImproperServiceDataException : AbapExceptions
        {

            public AbapImproperServiceDataException() : base(46, "AbapImproperServiceDataException")
            {
                
            }

            public override string ErrorDescription => "Service has improper data in SLD.";

            public override int AbapCode => 46;

            public override string AbapName => "ERROR_CODE_IMPROPER_DATA            ";
        }
        
        private sealed class AbapEmptyNoteException : AbapExceptions
        {

            public AbapEmptyNoteException() : base(47, "AbapEmptyNoteException")
            {
                
            }

            public override string ErrorDescription => "Service has improper data in SLD.";

            public override int AbapCode => 47;

            public override string AbapName => "ERROR_CODE_EMPTY_NOTE";
        }
        
        private sealed class AbapEmptyTaskIdException : AbapExceptions
        {

            public AbapEmptyTaskIdException() : base(48, "AbapEmptyTaskIdException")
            {
                
            }

            public override string ErrorDescription => "Empty task ID.";

            public override int AbapCode => 48;

            public override string AbapName => "ERROR_CODE_NULL_OR_EMPTY_TASK_ID ";
        }
        
        private sealed class AbapInvalidTaskFromIdException : AbapExceptions
        {

            public AbapInvalidTaskFromIdException() : base(49, "AbapInvalidTaskFromIdException")
            {
                
            }

            public override string ErrorDescription => "No task exists for the given task ID.";

            public override int AbapCode => 49;

            public override string AbapName => "ERROR_CODE_NO_SUCH_TASK";
        }
        
        private sealed class AbapIllegitIdChangeException : AbapExceptions
        {

            public AbapIllegitIdChangeException() : base(50, "AbapIllegitIdChangeException")
            {
                
            }

            public override string ErrorDescription => "Illegitimate change ID usage.";

            public override int AbapCode => 50;

            public override string AbapName => "ERROR_CODE_ILLEGITIMATE_CHANGE_ID_USAGE ";
        }
        
        private sealed class AbapInvalidValidationResultException : AbapExceptions
        {

            public AbapInvalidValidationResultException() : base(51, "AbapInvalidValidationResultException")
            {
                
            }

            public override string ErrorDescription => "No task exists for the given entity ID and entity type.";

            public override int AbapCode => 51;

            public override string AbapName => "ERROR_CODE_NO_SUCH_VALIDATIONRESULT  ";
        }
        
        private sealed class AbapInvalidEntityIdException : AbapExceptions
        {

            public AbapInvalidEntityIdException() : base(52, "AbapInvalidEntityIdException")
            {
                
            }

            public override string ErrorDescription => "Empty entity ID.";

            public override int AbapCode => 52;

            public override string AbapName => "ERROR_CODE_NULL_OR_EMPTY_ENTITY_ID";
        }
        
        private sealed class AbapInvalidTypeException : AbapExceptions
        {

            public AbapInvalidTypeException() : base(53, "AbapInvalidTypeException")
            {
                
            }

            public override string ErrorDescription => "Empty entity type.";

            public override int AbapCode => 53;

            public override string AbapName => "ERROR_CODE_NULL_OR_EMPTY_ENTITY_TYPE ";
        }
        
        private sealed class AbapInvalidStateException : AbapExceptions
        {

            public AbapInvalidStateException() : base(54, "AbapInvalidStateException")
            {
                
            }

            public override string ErrorDescription => "No process state is specified.";

            public override int AbapCode => 54;

            public override string AbapName => "ERROR_CODE_NULL_OR_EMPTY_PROCESS_STATE  ";
        }
        
        private sealed class AbapInvalidLicenseException : AbapExceptions
        {

            public AbapInvalidLicenseException() : base(55, "AbapInvalidLicenseException")
            {
                
            }

            public override string ErrorDescription => "Enterprise license is required but not installed.";

            public override int AbapCode => 55;

            public override string AbapName => "ERROR_CODE_ENTERPRISE_LICENSE_REQUIRED   ";
        }
        
        private sealed class AbapCustomParameterParsingException : AbapExceptions
        {

            public AbapCustomParameterParsingException() : base(56, "AbapCustomParameterParsingException")
            {
                
            }

            public override string ErrorDescription => "Error retrieving the replaced default values.";

            public override int AbapCode => 56;

            public override string AbapName => "ERROR_CODE_RETRIEVE_CUSTOM_PARAMETER    ";
        }
        

        #endregion       
    }
    
}