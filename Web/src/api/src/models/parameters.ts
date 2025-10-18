import {
  OperationParameter,
  OperationURLParameter,
  OperationQueryParameter,
} from "@azure/core-client";
import {
  LoginRequestDto as LoginRequestDtoMapper,
  RefreshTokenRequestDto as RefreshTokenRequestDtoMapper,
  UserCreateDto as UserCreateDtoMapper,
  UserUpdateDto as UserUpdateDtoMapper,
} from '../models/mappers';

export const contentType: OperationParameter = {
  parameterPath: ["options", "contentType"],
  mapper: {
    defaultValue: "application/json",
    isConstant: true,
    serializedName: "Content-Type",
    type: {
      name: "String",
    },
  },
};

export const body: OperationParameter = {
  parameterPath: ["options", "body"],
  mapper: LoginRequestDtoMapper,
};

export const accept: OperationParameter = {
  parameterPath: "accept",
  mapper: {
    defaultValue: "application/json, text/json",
    isConstant: true,
    serializedName: "Accept",
    type: {
      name: "String",
    },
  },
};

export const $host: OperationURLParameter = {
  parameterPath: "$host",
  mapper: {
    serializedName: "$host",
    required: true,
    type: {
      name: "String",
    },
  },
  skipEncoding: true,
};

export const xApiVersion: OperationParameter = {
  parameterPath: ["options", "xApiVersion"],
  mapper: {
    serializedName: "X-Api-Version",
    type: {
      name: "String",
    },
  },
};

export const apiVersion: OperationQueryParameter = {
  parameterPath: "apiVersion",
  mapper: {
    defaultValue: "v1",
    isConstant: true,
    serializedName: "api-version",
    type: {
      name: "String",
    },
  },
};

export const body1: OperationParameter = {
  parameterPath: ["options", "body"],
  mapper: RefreshTokenRequestDtoMapper,
};

export const body2: OperationParameter = {
  parameterPath: ["options", "body"],
  mapper: UserCreateDtoMapper,
};

export const nome: OperationQueryParameter = {
  parameterPath: ["options", "nome"],
  mapper: {
    serializedName: "nome",
    type: {
      name: "String",
    },
  },
};

export const body3: OperationParameter = {
  parameterPath: ["options", "body"],
  mapper: UserUpdateDtoMapper,
};

export const id: OperationURLParameter = {
  parameterPath: "id",
  mapper: {
    serializedName: "id",
    required: true,
    type: {
      name: "Uuid",
    },
  },
};
