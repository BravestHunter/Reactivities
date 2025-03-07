import axios, { AxiosError, AxiosResponse } from 'axios'
import { toast } from 'react-toastify'
import { router } from '../router/Routes'
import User from '../models/user'
import { Profile } from '../models/profile'
import Photo from '../models/photo'
import { sleep, toURLSearchParams } from '../utils'
import PagedList from '../models/pagedist'
import AccessToken from '../models/accessToken'
import ActivityFormValues from '../models/forms/activityFormValues'
import RegisterRequest from '../models/requests/registerRequest'
import LoginRequest from '../models/requests/loginRequest'
import ActivityDto from '../models/dtos/activityDto'
import { globalStore } from '../stores/globalStore'
import ServerError from '../models/serverError'
import { GetActivitiesRequest } from '../models/requests/getActivitiesRequest'
import ProfileFormValues from '../models/forms/profileFormValues'

axios.defaults.baseURL = import.meta.env.VITE_API_URL
axios.defaults.withCredentials = true

const responseBody = <T>(response: AxiosResponse<T>) => response.data

axios.interceptors.request.use((config) => {
  const token = globalStore.commonStore.token
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

axios.interceptors.response.use(
  async (response) => {
    if (import.meta.env.MODE === 'development') {
      // Fake delay
      await sleep(1000)
    }

    return response
  },
  (error: AxiosError) => {
    const response = error.response as AxiosResponse
    const { data, status, config, headers } = response

    switch (status) {
      case 400:
        if (data.errors) {
          if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
            router.navigate('/notfound')
          } else {
            const modalStateErrors = Object.values(data.errors)
            throw modalStateErrors.flat()
          }
        } else {
          toast.error(data)
        }
        break

      case 401:
        if (
          headers['www-authenticate']?.startsWith(
            'Bearer error="invalid_token"'
          )
        ) {
          globalStore.userStore.logout()
          toast.error('Session Expired')
          return
        }
        toast.error('Unauthorized')
        break

      case 403:
        toast.error('Forbidden')
        break

      case 404:
        router.navigate('/notfound')
        break

      case 500:
        const serverError = responseBody<ServerError>(response)
        if (serverError) {
          globalStore.commonStore.setServerError(serverError)
        }
        router.navigate('/servererror')
        break
    }

    return Promise.reject(error)
  }
)

const requests = {
  get: <T>(
    url: string,
    searchParams: URLSearchParams | undefined = undefined
  ) => axios.get<T>(url, { params: searchParams }).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Account = {
  register: (request: RegisterRequest) =>
    requests.post<User>('/account/register', request),
  login: (request: LoginRequest) =>
    requests.post<User>('/account/login', request),
  refreshToken: () => requests.get<AccessToken>('/account/refreshToken'),
  current: () => requests.get<User>('/account'),
}

const Profiles = {
  get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
  update: (profile: ProfileFormValues) =>
    requests.put<Profile>('/profiles', profile),
  listActivities: (username: string, request: GetActivitiesRequest) =>
    requests.get<PagedList<ActivityDto>>(
      `/profiles/${username}/activities`,
      toURLSearchParams(request)
    ),
  listFollowings: (username: string, predicate: string) =>
    requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`),
  updateFollowing: (username: string) =>
    requests.post(`/follow/${username}`, {}),
  uploadPhoto: (file: Blob) => {
    let formData = new FormData()
    formData.append('File', file)

    return axios.post<Photo>('/photos', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
  },
  setProfilePhoto: (id: number) =>
    requests.post(`/photos/${id}/setProfile`, {}),
  deletePhoto: (id: number) => requests.delete(`/photos/${id}`),
}

const Activities = {
  list: (request: GetActivitiesRequest) =>
    requests.get<PagedList<ActivityDto>>(
      '/activities',
      toURLSearchParams(request)
    ),
  details: (id: number) => requests.get<ActivityDto>(`/activities/${id}`),
  create: (activity: ActivityFormValues) =>
    requests.post<ActivityDto>('/activities', activity),
  update: (activity: ActivityFormValues) =>
    requests.put<ActivityDto>(`/activities/${activity.id}`, activity),
  delete: (id: number) => requests.delete<void>(`/activities/${id}`),
  updateAttendance: (id: number, attend: boolean) =>
    requests.post<void>(
      `/activities/${id}/updateAttendance?attend=${attend}`,
      {}
    ),
}

const agent = {
  Account,
  Profiles,
  Activities,
}

export default agent
