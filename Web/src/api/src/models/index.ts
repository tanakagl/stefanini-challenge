import * as coreClient from "@azure/core-client";

export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface LoginResponseDto {
  accessToken?: string;
  refreshToken?: string;
  expiresAt?: Date;
  user?: UserInfoDto;
}

export interface UserInfoDto {
  id?: string;
  nomeCompleto?: string;
  email?: string;
}

export interface ProblemDetails {
  /** Describes unknown properties. The value of an unknown property can be of "any" type. */
  [property: string]: any;
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
}

export interface RefreshTokenRequestDto {
  refreshToken: string;
}

export interface UserResponseDto {
  id?: string;
  nomeCompleto?: string;
  sexo?: SexoUsuario;
  email?: string;
  dataNascimento?: Date;
  nacionalidade?: string;
  naturalidade?: string;
  cpf?: string;
  dataCriacao?: Date;
  dataUltimaAtualizacao?: Date;
}

export interface UserCreateDto {
  nomeCompleto: string;
  sexo?: SexoUsuario;
  email?: string;
  dataNascimento: Date;
  nacionalidade: string;
  naturalidade: string;
  cpf: string;
  password: string;
}

export interface UserUpdateDto {
  nomeCompleto: string;
  sexo?: SexoUsuario;
  email?: string;
  dataNascimento: Date;
  nacionalidade?: string;
  naturalidade?: string;
  cpf: string;
}

/** Known values of {@link SexoUsuario} that the service accepts. */
export enum KnownSexoUsuario {
  /** Masculino */
  Masculino = "Masculino",
  /** Feminino */
  Feminino = "Feminino",
  /** Outro */
  Outro = "Outro",
}

/**
 * Defines values for SexoUsuario. \
 * {@link KnownSexoUsuario} can be used interchangeably with SexoUsuario,
 *  this enum contains the known values that the service supports.
 * ### Known values supported by the service
 * **Masculino** \
 * **Feminino** \
 * **Outro**
 */
export type SexoUsuario = string;

/** Optional parameters. */
export interface AuthLoginPostOptionalParams
  extends coreClient.OperationOptions {
  body?: LoginRequestDto;
  xApiVersion?: string;
}

/** Contains response data for the authLoginPost operation. */
export type AuthLoginPostResponse = LoginResponseDto;

/** Optional parameters. */
export interface AuthRefreshPostOptionalParams
  extends coreClient.OperationOptions {
  xApiVersion?: string;
  body?: RefreshTokenRequestDto;
}

/** Contains response data for the authRefreshPost operation. */
export type AuthRefreshPostResponse = LoginResponseDto;

/** Optional parameters. */
export interface AuthMeGetOptionalParams extends coreClient.OperationOptions {
  xApiVersion?: string;
}

/** Contains response data for the authMeGet operation. */
export type AuthMeGetResponse = UserInfoDto;

/** Optional parameters. */
export interface GetAllUsersOptionalParams
  extends coreClient.OperationOptions {}

/** Contains response data for the getAllUsers operation. */
export type GetAllUsersResponse = UserResponseDto[];

/** Optional parameters. */
export interface CreateUserOptionalParams extends coreClient.OperationOptions {
  body?: UserCreateDto;
}

/** Contains response data for the createUser operation. */
export type CreateUserResponse = UserResponseDto;

/** Optional parameters. */
export interface GetUsersByNameOptionalParams
  extends coreClient.OperationOptions {
  nome?: string;
}

/** Contains response data for the getUsersByName operation. */
export type GetUsersByNameResponse = UserResponseDto[];

/** Optional parameters. */
export interface UpdateUserOptionalParams extends coreClient.OperationOptions {
  body?: UserUpdateDto;
}

/** Contains response data for the updateUser operation. */
export type UpdateUserResponse = UserResponseDto;

/** Optional parameters. */
export interface DeleteUserOptionalParams extends coreClient.OperationOptions {}

/** Contains response data for the deleteUser operation. */
export type DeleteUserResponse = ProblemDetails;

/** Optional parameters. */
export interface ApiClientOptionalParams
  extends coreClient.ServiceClientOptions {
  /** Api Version */
  apiVersion?: string;
  /** Overrides client endpoint. */
  endpoint?: string;
}
