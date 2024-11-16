import { makeAutoObservable, reaction, runInAction } from 'mobx'
import { Activity, ActivityFormValues } from '../models/activity'
import agent from '../api/agent'
import { format } from 'date-fns'
import { store } from './store'
import { Profile } from '../models/profile'
import PagingParams from '../models/pagingParams'
import PageParams from '../models/pageParams'

export default class ActivityStore {
  activityRegistry: Map<number, Activity> = new Map<number, Activity>()
  selectedActivity: Activity | undefined = undefined
  loading: boolean = false
  loadingInitial: boolean = false
  pageParams: PageParams | null = null
  pagingParams: PagingParams = new PagingParams()
  predicate: Map<string, any> = new Map<string, any>().set('all', true)

  constructor() {
    makeAutoObservable(this)

    reaction(
      () => this.predicate.keys(),
      () => {
        this.pagingParams = new PagingParams()
        this.activityRegistry.clear()
        this.loadActivities()
      }
    )
  }

  private getActivity = (id: number) => {
    return this.activityRegistry.get(id)
  }

  private setActivity = (activity: Activity) => {
    const user = store.userStore.user
    if (user) {
      activity.isGoing = activity.attendees!.some(
        (a) => a.username === user.username
      )
      activity.isHost = activity.host.username === user.username
    }

    activity.date = new Date(activity.date!)
    this.activityRegistry.set(activity.id, activity)
  }

  get activitiesByDate() {
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

  get axiosParams() {
    const params = new URLSearchParams()
    params.append('pageNumber', this.pagingParams.pageNumber.toString())
    params.append('pageSize', this.pagingParams.pageSize.toString())

    this.predicate.forEach((value, key) => {
      if (key === 'startDate') {
        params.append('fromDate', (value as Date).toISOString())
      } else {
        params.append(key, value)
      }
    })

    return params
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

  setPagingParams = (pagingParams: PagingParams) => {
    this.pagingParams = pagingParams
  }

  setPredicate = (predicate: string, value: string | Date) => {
    const resetPredicate = () => {
      this.predicate.forEach((value, key) => {
        if (key !== 'startDate') {
          this.predicate.delete(key)
        }
      })
    }

    switch (predicate) {
      case 'all':
        resetPredicate()
        this.predicate.set('all', true)
        break

      case 'isGoing':
        resetPredicate()
        this.predicate.set('isGoing', true)
        break

      case 'isHost':
        resetPredicate()
        this.predicate.set('isHost', true)
        break

      case 'startDate':
        this.predicate.delete('startDate')
        this.predicate.set('startDate', value)
        break
    }
  }

  loadActivities = async () => {
    this.setLoadingInitial(true)

    try {
      const result = await agent.Activities.list(this.axiosParams)

      result.items.forEach((a) => {
        this.setActivity(a)
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
        activity = await agent.Activities.details(id)
        this.setActivity(activity)
      } catch (error) {
        console.log(error)
      } finally {
        this.setLoadingInitial(false)
      }
    }

    runInAction(() => (this.selectedActivity = activity))
    return activity
  }

  createActivity = async (activity: ActivityFormValues) => {
    const user = store.userStore.user
    //const attendee = new Profile(user!)

    try {
      const newActivity = await agent.Activities.create(activity)
      //newActivity.hostUsername = user!.username
      //newActivity.attendees = [attendee]
      this.setActivity(newActivity)

      runInAction(() => {
        this.selectedActivity = newActivity
      })
    } catch (error) {
      console.log(error)
    }
  }

  updateActivity = async (ActivityFormValues: ActivityFormValues) => {
    try {
      var activity = await agent.Activities.update(ActivityFormValues)

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
    const user = store.userStore.user
    if (!this.selectedActivity || !user) {
      return
    }

    this.setLoading(true)

    try {
      await agent.Activities.attend(this.selectedActivity.id)
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
    this.setLoading(true)

    try {
      await agent.Activities.attend(this.selectedActivity!.id)

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
