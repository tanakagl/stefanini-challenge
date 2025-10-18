import * as coreClient from "@azure/core-client";

export const LoginRequestDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "LoginRequestDto",
    modelProperties: {
      email: {
        constraints: {
          MinLength: 1,
        },
        serializedName: "email",
        required: true,
        type: {
          name: "String",
        },
      },
      password: {
        constraints: {
          MinLength: 1,
        },
        serializedName: "password",
        required: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const LoginResponseDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "LoginResponseDto",
    modelProperties: {
      accessToken: {
        serializedName: "accessToken",
        nullable: true,
        type: {
          name: "String",
        },
      },
      refreshToken: {
        serializedName: "refreshToken",
        nullable: true,
        type: {
          name: "String",
        },
      },
      expiresAt: {
        serializedName: "expiresAt",
        type: {
          name: "DateTime",
        },
      },
      user: {
        serializedName: "user",
        type: {
          name: "Composite",
          className: "UserInfoDto",
        },
      },
    },
  },
};

export const UserInfoDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "UserInfoDto",
    modelProperties: {
      id: {
        serializedName: "id",
        type: {
          name: "Uuid",
        },
      },
      nomeCompleto: {
        serializedName: "nomeCompleto",
        nullable: true,
        type: {
          name: "String",
        },
      },
      email: {
        serializedName: "email",
        nullable: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const ProblemDetails: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "ProblemDetails",
    additionalProperties: { type: { name: "Object" } },
    modelProperties: {
      type: {
        serializedName: "type",
        nullable: true,
        type: {
          name: "String",
        },
      },
      title: {
        serializedName: "title",
        nullable: true,
        type: {
          name: "String",
        },
      },
      status: {
        serializedName: "status",
        nullable: true,
        type: {
          name: "Number",
        },
      },
      detail: {
        serializedName: "detail",
        nullable: true,
        type: {
          name: "String",
        },
      },
      instance: {
        serializedName: "instance",
        nullable: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const RefreshTokenRequestDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "RefreshTokenRequestDto",
    modelProperties: {
      refreshToken: {
        constraints: {
          MinLength: 1,
        },
        serializedName: "refreshToken",
        required: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const UserResponseDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "UserResponseDto",
    modelProperties: {
      id: {
        serializedName: "id",
        type: {
          name: "Uuid",
        },
      },
      nomeCompleto: {
        serializedName: "nomeCompleto",
        nullable: true,
        type: {
          name: "String",
        },
      },
      sexo: {
        serializedName: "sexo",
        type: {
          name: "String",
        },
      },
      email: {
        serializedName: "email",
        nullable: true,
        type: {
          name: "String",
        },
      },
      dataNascimento: {
        serializedName: "dataNascimento",
        type: {
          name: "DateTime",
        },
      },
      nacionalidade: {
        serializedName: "nacionalidade",
        nullable: true,
        type: {
          name: "String",
        },
      },
      naturalidade: {
        serializedName: "naturalidade",
        nullable: true,
        type: {
          name: "String",
        },
      },
      cpf: {
        serializedName: "cpf",
        nullable: true,
        type: {
          name: "String",
        },
      },
      dataCriacao: {
        serializedName: "dataCriacao",
        type: {
          name: "DateTime",
        },
      },
      dataUltimaAtualizacao: {
        serializedName: "dataUltimaAtualizacao",
        type: {
          name: "DateTime",
        },
      },
    },
  },
};

export const UserCreateDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "UserCreateDto",
    modelProperties: {
      nomeCompleto: {
        constraints: {
          MaxLength: 200,
          MinLength: 3,
        },
        serializedName: "nomeCompleto",
        required: true,
        type: {
          name: "String",
        },
      },
      sexo: {
        serializedName: "sexo",
        type: {
          name: "String",
        },
      },
      email: {
        serializedName: "email",
        nullable: true,
        type: {
          name: "String",
        },
      },
      dataNascimento: {
        serializedName: "dataNascimento",
        required: true,
        type: {
          name: "DateTime",
        },
      },
      nacionalidade: {
        constraints: {
          MaxLength: 100,
          MinLength: 2,
        },
        serializedName: "nacionalidade",
        required: true,
        type: {
          name: "String",
        },
      },
      naturalidade: {
        constraints: {
          MaxLength: 100,
          MinLength: 2,
        },
        serializedName: "naturalidade",
        required: true,
        type: {
          name: "String",
        },
      },
      cpf: {
        constraints: {
          MinLength: 1,
        },
        serializedName: "cpf",
        required: true,
        type: {
          name: "String",
        },
      },
      password: {
        constraints: {
          MaxLength: 100,
          MinLength: 6,
        },
        serializedName: "password",
        required: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const UserUpdateDto: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "UserUpdateDto",
    modelProperties: {
      nomeCompleto: {
        constraints: {
          MaxLength: 200,
          MinLength: 3,
        },
        serializedName: "nomeCompleto",
        required: true,
        type: {
          name: "String",
        },
      },
      sexo: {
        serializedName: "sexo",
        type: {
          name: "String",
        },
      },
      email: {
        serializedName: "email",
        nullable: true,
        type: {
          name: "String",
        },
      },
      dataNascimento: {
        serializedName: "dataNascimento",
        required: true,
        type: {
          name: "DateTime",
        },
      },
      nacionalidade: {
        serializedName: "nacionalidade",
        nullable: true,
        type: {
          name: "String",
        },
      },
      naturalidade: {
        serializedName: "naturalidade",
        nullable: true,
        type: {
          name: "String",
        },
      },
      cpf: {
        constraints: {
          MinLength: 1,
        },
        serializedName: "cpf",
        required: true,
        type: {
          name: "String",
        },
      },
    },
  },
};
