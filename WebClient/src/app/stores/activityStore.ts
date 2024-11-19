import { makeAutoObservable, reaction, runInAction } from 'mobx'
import agent from '../api/agent'
import { format } from 'date-fns'
import { Profile } from '../models/profile'
import PagingParams from '../models/pagingParams'
import PageParams from '../models/pageParams'
import { Activity } from '../models/activity'
import ActivityFormValues from '../models/forms/activityFormValues'
import ActivityDto from '../models/dtos/activityDto'
import { globalStore } from './globalStore'
import { ActivityListFilters } from '../models/activityListFilters'
import { GetActivitiesRequest } from '../models/requests/getActivitiesRequest'

export default class ActivityStore {
  activityRegistry: Map<number, Activity> = new Map<number, Activity>()
  selectedActivity: Activity | undefined = undefined
  loading: boolean = false
  loadingInitial: boolean = false
  pageParams: PageParams | null = null

  listFilters: ActivityListFilters = new ActivityListFilters()

  constructor() {
    makeAutoObservable(this)

    reaction(
      () => ({
        fromDate: this.listFilters.fromDate,
        relationship: this.listFilters.relationship,
      }),
      this.resetActivitiesList
    )
  }

  get currentPage() {
    return this.pageParams?.currentPage ?? 1
  }

  get hasMore() {
    return (
      (this.pageParams &&
        this.pageParams.currentPage < this.pageParams.totalPages) ||
      false
    )
  }

  setListFilter = <K extends keyof ActivityListFilters>(
    key: K,
    value: ActivityListFilters[K]
  ) => {
    this.listFilters[key] = value
  }

  private getActivity = (id: number) => {
    return this.activityRegistry.get(id)
  }

  private addActivity = (activityDto: ActivityDto) => {
    const user = globalStore.userStore.user
    const activity = new Activity(activityDto, user)
    this.activityRegistry.set(activity.id, activity)

    return activity
  }

  private resetActivitiesList = () => {
    this.activityRegistry.clear()
    this.selectedActivity = undefined
    this.loadActivities(1)
  }

  private get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => a.date!.getTime() - b.date!.getTime()
    )
  }

  get groupedActivities() {
    return Object.entries(
      this.activitiesByDate.reduce((activities, activity) => {
        const date = format(activity.date!, 'dd MMM yyyy')
        activities[date] = activities[date]
          ? [...activities[date], activity]
          : [activity]
        return activities
      }, {} as { [key: string]: Activity[] })
    )
  }

  setLoadingInitial = (value: boolean) => {
    this.loadingInitial = value
  }

  setLoading = (value: boolean) => {
    this.loading = value
  }

  setPageParams = (params: PageParams) => {
    this.pageParams = params
  }

  loadActivities = async (pageNumber: number) => {
    this.setLoadingInitial(true)

    try {
      const pagingParams = new PagingParams(pageNumber)
      const request = new GetActivitiesRequest(pagingParams, this.listFilters)
      const result = await agent.Activities.list(request)

      result.items.forEach((a) => {
        this.addActivity(a)
      })
      this.setPageParams(result.params)
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoadingInitial(false)
    }
  }

  loadActivity = async (id: number) => {
    let activity = this.getActivity(id)

    if (!activity) {
      this.setLoadingInitial(true)

      try {
        const activityDto = await agent.Activities.details(id)
        activity = this.addActivity(activityDto)
      } catch (error) {
        console.log(error)
      } finally {
        this.setLoadingInitial(false)
      }
    }

    runInAction(() => (this.selectedActivity = activity))
    return activity
  }

  createActivity = async (formValues: ActivityFormValues) => {
    try {
      const activityDto = await agent.Activities.create(formValues)
      const activity = this.addActivity(activityDto)

      runInAction(() => {
        this.selectedActivity = activity
      })
    } catch (error) {
      console.log(error)
    }
  }

  updateActivity = async (formValues: ActivityFormValues) => {
    try {
      var activity = await agent.Activities.update(formValues)

      runInAction(() => {
        if (activity.id) {
          let updatedActivity = {
            ...this.getActivity(activity.id),
            ...activity,
          }
          this.activityRegistry.set(activity.id, updatedActivity as Activity)
          this.selectedActivity = updatedActivity as Activity
        }
      })
    } catch (error) {
      console.log(error)
    }
  }

  deleteActivity = async (id: number) => {
    this.setLoading(true)

    try {
      await agent.Activities.delete(id)

      runInAction(() => {
        this.activityRegistry.delete(id)
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  updateAttendance = async () => {
    const user = globalStore.userStore.user
    if (!this.selectedActivity || !user) {
      return
    }

    this.setLoading(true)

    try {
      const isGoing = this.selectedActivity.isGoing
      await agent.Activities.updateAttendance(
        this.selectedActivity.id,
        !isGoing
      )
      this.selectedActivity.isGoing = !isGoing

      runInAction(() => {
        if (this.selectedActivity?.isGoing) {
          this.selectedActivity.attendees =
            this.selectedActivity.attendees?.filter(
              (a) => a.username !== user.username
            )

          this.selectedActivity.isGoing = false
        } else {
          const attendee = new Profile(user)
          this.selectedActivity!.attendees?.push(attendee)
          this.selectedActivity!.isGoing = true
        }
        this.activityRegistry.set(
          this.selectedActivity!.id,
          this.selectedActivity!
        )
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  cancelActivityToggle = async () => {
    const user = globalStore.userStore.user
    if (!this.selectedActivity || !user) {
      return
    }

    this.setLoading(true)

    try {
      const isGoing = this.selectedActivity.isGoing
      await agent.Activities.updateAttendance(
        this.selectedActivity!.id,
        !isGoing
      )

      runInAction(() => {
        this.selectedActivity!.isCancelled = !this.selectedActivity?.isCancelled
        this.activityRegistry.set(
          this.selectedActivity!.id,
          this.selectedActivity!
        )
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  clearSelectedActivity = () => {
    this.selectedActivity = undefined
  }

  updateAttendeeFollowing = (username: string) => {
    this.activityRegistry.forEach((activity) => {
      activity.attendees?.forEach((attendee) => {
        if (attendee.username === username) {
          attendee.following
            ? attendee.followersCount--
            : attendee.followingCount++
          attendee.following = !attendee.following
        }
      })
    })
  }
}
