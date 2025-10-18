import * as coreClient from "@azure/core-client";
import * as coreRestPipeline from "@azure/core-rest-pipeline";
import {
  PipelineRequest,
  PipelineResponse,
  SendRequest,
} from "@azure/core-rest-pipeline";
import * as coreAuth from "@azure/core-auth";
import * as Parameters from './models/parameters';
import * as Mappers from './models/mappers';
import {
  ApiClientOptionalParams,
  AuthLoginPostOptionalParams,
  AuthLoginPostResponse,
  AuthRefreshPostOptionalParams,
  AuthRefreshPostResponse,
  AuthMeGetOptionalParams,
  AuthMeGetResponse,
  GetAllUsersOptionalParams,
  GetAllUsersResponse,
  CreateUserOptionalParams,
  CreateUserResponse,
  GetUsersByNameOptionalParams,
  GetUsersByNameResponse,
  UpdateUserOptionalParams,
  UpdateUserResponse,
  DeleteUserOptionalParams,
  DeleteUserResponse,
} from './models/index';

export class ApiClient extends coreClient.ServiceClient {
  $host: string;
  apiVersion: string;

  /**
   * Initializes a new instance of the ApiClient class.
   * @param credentials Subscription credentials which uniquely identify client subscription.
   * @param $host server parameter
   * @param options The parameter options
   */
  constructor(
    credentials: coreAuth.TokenCredential,
    $host: string,
    options?: ApiClientOptionalParams,
  ) {
    if (credentials === undefined) {
      throw new Error("'credentials' cannot be null");
    }
    if ($host === undefined) {
      throw new Error("'$host' cannot be null");
    }

    // Initializing default values for options
    if (!options) {
      options = {};
    }
    const defaults: ApiClientOptionalParams = {
      requestContentType: "application/json; charset=utf-8",
      credential: credentials,
    };

    const packageDetails = `azsdk-js-apiClient/1.0.0-beta.1`;
    const userAgentPrefix =
      options.userAgentOptions && options.userAgentOptions.userAgentPrefix
        ? `${options.userAgentOptions.userAgentPrefix} ${packageDetails}`
        : `${packageDetails}`;

    const optionsWithDefaults = {
      ...defaults,
      ...options,
      userAgentOptions: {
        userAgentPrefix,
      },
      endpoint: options.endpoint ?? options.baseUri ?? "{$host}",
    };
    super(optionsWithDefaults);

    let bearerTokenAuthenticationPolicyFound: boolean = false;
    if (options?.pipeline && options.pipeline.getOrderedPolicies().length > 0) {
      const pipelinePolicies: coreRestPipeline.PipelinePolicy[] =
        options.pipeline.getOrderedPolicies();
      bearerTokenAuthenticationPolicyFound = pipelinePolicies.some(
        (pipelinePolicy) =>
          pipelinePolicy.name ===
          coreRestPipeline.bearerTokenAuthenticationPolicyName,
      );
    }
    if (
      !options ||
      !options.pipeline ||
      options.pipeline.getOrderedPolicies().length == 0 ||
      !bearerTokenAuthenticationPolicyFound
    ) {
      this.pipeline.removePolicy({
        name: coreRestPipeline.bearerTokenAuthenticationPolicyName,
      });
      this.pipeline.addPolicy(
        coreRestPipeline.bearerTokenAuthenticationPolicy({
          credential: credentials,
          scopes:
            optionsWithDefaults.credentialScopes ??
            `${optionsWithDefaults.endpoint}/.default`,
          challengeCallbacks: {
            authorizeRequestOnChallenge:
              coreClient.authorizeRequestOnClaimChallenge,
          },
        }),
      );
    }
    // Parameter assignments
    this.$host = $host;

    // Assigning values to Constant parameters
    this.apiVersion = options.apiVersion || "v1";
    this.addCustomApiVersionPolicy(options.apiVersion);
  }

  /** A function that adds a policy that sets the api-version (or equivalent) to reflect the library version. */
  private addCustomApiVersionPolicy(apiVersion?: string) {
    if (!apiVersion) {
      return;
    }
    const apiVersionPolicy = {
      name: "CustomApiVersionPolicy",
      async sendRequest(
        request: PipelineRequest,
        next: SendRequest,
      ): Promise<PipelineResponse> {
        const param = request.url.split("?");
        if (param.length > 1) {
          const newParams = param[1].split("&").map((item) => {
            if (item.indexOf("api-version") > -1) {
              return "api-version=" + apiVersion;
            } else {
              return item;
            }
          });
          request.url = param[0] + "?" + newParams.join("&");
        }
        return next(request);
      },
    };
    this.pipeline.addPolicy(apiVersionPolicy);
  }

  /** @param options The options parameters. */
  authLoginPost(
    options?: AuthLoginPostOptionalParams,
  ): Promise<AuthLoginPostResponse> {
    return this.sendOperationRequest({ options }, authLoginPostOperationSpec);
  }

  /** @param options The options parameters. */
  authRefreshPost(
    options?: AuthRefreshPostOptionalParams,
  ): Promise<AuthRefreshPostResponse> {
    return this.sendOperationRequest({ options }, authRefreshPostOperationSpec);
  }

  /** @param options The options parameters. */
  authMeGet(options?: AuthMeGetOptionalParams): Promise<AuthMeGetResponse> {
    return this.sendOperationRequest({ options }, authMeGetOperationSpec);
  }

  /** @param options The options parameters. */
  getAllUsers(
    options?: GetAllUsersOptionalParams,
  ): Promise<GetAllUsersResponse> {
    return this.sendOperationRequest({ options }, getAllUsersOperationSpec);
  }

  /** @param options The options parameters. */
  createUser(options?: CreateUserOptionalParams): Promise<CreateUserResponse> {
    return this.sendOperationRequest({ options }, createUserOperationSpec);
  }

  /** @param options The options parameters. */
  getUsersByName(
    options?: GetUsersByNameOptionalParams,
  ): Promise<GetUsersByNameResponse> {
    return this.sendOperationRequest({ options }, getUsersByNameOperationSpec);
  }

  /**
   * @param id
   * @param options The options parameters.
   */
  updateUser(
    id: string,
    options?: UpdateUserOptionalParams,
  ): Promise<UpdateUserResponse> {
    return this.sendOperationRequest({ id, options }, updateUserOperationSpec);
  }

  /**
   * @param id
   * @param options The options parameters.
   */
  deleteUser(
    id: string,
    options?: DeleteUserOptionalParams,
  ): Promise<DeleteUserResponse> {
    return this.sendOperationRequest({ id, options }, deleteUserOperationSpec);
  }
}
// Operation Specifications
const serializer = coreClient.createSerializer(Mappers, /* isXml */ false);

const authLoginPostOperationSpec: coreClient.OperationSpec = {
  path: "/api/Auth/login",
  httpMethod: "POST",
  responses: {
    200: {
      bodyMapper: Mappers.LoginResponseDto,
    },
    401: {
      bodyMapper: Mappers.ProblemDetails,
    },
  },
  requestBody: Parameters.body,
  queryParameters: [Parameters.apiVersion],
  urlParameters: [Parameters.$host],
  headerParameters: [
    Parameters.contentType,
    Parameters.accept,
    Parameters.xApiVersion,
  ],
  mediaType: "json",
  serializer,
};
const authRefreshPostOperationSpec: coreClient.OperationSpec = {
  path: "/api/Auth/refresh",
  httpMethod: "POST",
  responses: {
    200: {
      bodyMapper: Mappers.LoginResponseDto,
    },
    401: {
      bodyMapper: Mappers.ProblemDetails,
    },
  },
  requestBody: Parameters.body1,
  queryParameters: [Parameters.apiVersion],
  urlParameters: [Parameters.$host],
  headerParameters: [
    Parameters.contentType,
    Parameters.accept,
    Parameters.xApiVersion,
  ],
  mediaType: "json",
  serializer,
};
const authMeGetOperationSpec: coreClient.OperationSpec = {
  path: "/api/Auth/me",
  httpMethod: "GET",
  responses: {
    200: {
      bodyMapper: Mappers.UserInfoDto,
    },
    401: {
      bodyMapper: Mappers.ProblemDetails,
    },
  },
  queryParameters: [Parameters.apiVersion],
  urlParameters: [Parameters.$host],
  headerParameters: [Parameters.accept, Parameters.xApiVersion],
  serializer,
};
const getAllUsersOperationSpec: coreClient.OperationSpec = {
  path: "/api/v1/Users",
  httpMethod: "GET",
  responses: {
    200: {
      bodyMapper: {
        type: {
          name: "Sequence",
          element: {
            type: { name: "Composite", className: "UserResponseDto" },
          },
        },
      },
    },
  },
  urlParameters: [Parameters.$host],
  headerParameters: [Parameters.accept],
  serializer,
};
const createUserOperationSpec: coreClient.OperationSpec = {
  path: "/api/v1/Users",
  httpMethod: "POST",
  responses: {
    201: {
      bodyMapper: Mappers.UserResponseDto,
    },
    400: {
      bodyMapper: Mappers.ProblemDetails,
    },
  },
  requestBody: Parameters.body2,
  urlParameters: [Parameters.$host],
  headerParameters: [Parameters.contentType, Parameters.accept],
  mediaType: "json",
  serializer,
};
const getUsersByNameOperationSpec: coreClient.OperationSpec = {
  path: "/api/v1/Users/search",
  httpMethod: "GET",
  responses: {
    200: {
      bodyMapper: {
        type: {
          name: "Sequence",
          element: {
            type: { name: "Composite", className: "UserResponseDto" },
          },
        },
      },
    },
  },
  queryParameters: [Parameters.nome],
  urlParameters: [Parameters.$host],
  headerParameters: [Parameters.accept],
  serializer,
};
const updateUserOperationSpec: coreClient.OperationSpec = {
  path: "/api/v1/Users/{id}",
  httpMethod: "PUT",
  responses: {
    200: {
      bodyMapper: Mappers.UserResponseDto,
    },
    400: {
      bodyMapper: Mappers.ProblemDetails,
    },
    404: {
      bodyMapper: Mappers.ProblemDetails,
    },
  },
  requestBody: Parameters.body3,
  urlParameters: [Parameters.$host, Parameters.id],
  headerParameters: [Parameters.contentType, Parameters.accept],
  mediaType: "json",
  serializer,
};
const deleteUserOperationSpec: coreClient.OperationSpec = {
  path: "/api/v1/Users/{id}",
  httpMethod: "DELETE",
  responses: {
    204: {},
    404: {
      bodyMapper: Mappers.ProblemDetails,
    },
  },
  urlParameters: [Parameters.$host, Parameters.id],
  headerParameters: [Parameters.accept],
  serializer,
};
